#!/usr/bin/env python3
"""Parse-check the Unity Android Gradle templates without running Unity.

Renders every template with both fully populated and empty Unity tokens, then
parses the result with Groovy so malformed placeholders or Gradle syntax errors
are caught before a Cloud Build run.

Usage: GROOVY=/path/to/groovy python3 Tools/validate_gradle_templates.py
"""
import os
import re
import subprocess
import sys
import tempfile

BASE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..",
                    "Assets", "Plugins", "Android")
GROOVY = os.environ.get("GROOVY", "groovy")

FULL = {
    "APPLY_PLUGINS": "apply plugin: 'com.google.gms.google-services'",
    "DEPS": "implementation(name: 'a', ext: 'aar')\n    implementation 'androidx.appcompat:appcompat:1.6.1'",
    "APIVERSION": "35",
    "BUILDTOOLS": "34.0.0",
    "MINSDKVERSION": "26",
    "TARGETSDKVERSION": "35",
    "APPLICATIONID": "com.altomedia.castlekingdomseason",
    "ABIFILTERS": "'armeabi-v7a', 'arm64-v8a'",
    "VERSIONCODE": "1",
    "VERSIONNAME": "1.0",
    "USER_PROGUARD": ", 'proguard-user.txt'",
    "MINIFY_DEBUG": "false",
    "MINIFY_RELEASE": "true",
    "NDKPATH": "C:/ndk",
    "SIGN": "\n\n    signingConfigs {\n        release {\n            storeFile file('x.keystore')\n            storePassword 'p'\n            keyAlias 'k'\n            keyPassword 'p'\n        }\n    }",
    "SIGNCONFIG": "\n            signingConfig signingConfigs.release",
    "PACKAGING_OPTIONS": "packagingOptions {\n        doNotStrip '*/armeabi-v7a/*.so'\n    }",
    "PLAY_ASSET_PACKS": "assetPacks = [':pack1']",
    "SPLITS": "splits {\n        abi {\n            enable true\n            reset()\n            include 'armeabi-v7a'\n        }\n    }",
    "SPLITS_VERSION_CODE": "ext.abiCodes = ['armeabi-v7a': 1]",
    "EXTERNAL_SOURCES": "",
    "BUILD_SCRIPT_DEPS": "classpath 'com.google.gms:google-services:4.3.15'",
    "ARTIFACTORYREPOSITORY": "",
    "INCLUDES": "include ':foo'",
}
EMPTY = {k: "" for k in FULL}
TEMPLATES = ["launcherTemplate.gradle", "mainTemplate.gradle",
             "baseProjectTemplate.gradle", "settingsTemplate.gradle"]


def render(path, table):
    s = open(path).read()
    for key, value in table.items():
        s = s.replace("**%s**" % key, value)
    return s


def main():
    ok = True
    workdir = tempfile.mkdtemp(prefix="gradle-templates-")

    for name in TEMPLATES:
        path = os.path.join(BASE, name)
        for label, table in [("full", FULL), ("empty", EMPTY)]:
            rendered = render(path, table)
            leftover = re.findall(r"\*\*[A-Za-z_]+\*\*", rendered)
            # unityStreamingAssets is injected by Unity into gradle.properties
            prelude = "def unityStreamingAssets = ''\n" if "Template.gradle" in name and name.startswith(("launcher", "main")) else ""
            tmp = os.path.join(workdir, "%s_%s.gradle" % (name, label))
            open(tmp, "w").write(prelude + rendered)

            result = subprocess.run(
                [GROOVY, "-e",
                 "new groovy.lang.GroovyShell().parse(new File('%s'))" % tmp],
                capture_output=True, text=True)

            status = "OK" if result.returncode == 0 else "PARSE FAIL"
            if result.returncode != 0 or leftover:
                ok = False
            print(name, label, status, "leftover tokens:", leftover)
            if result.returncode != 0:
                print(result.stdout[:800], result.stderr[:800])

    print("ALL OK" if ok else "PROBLEMS")
    return 0 if ok else 1


if __name__ == "__main__":
    sys.exit(main())
