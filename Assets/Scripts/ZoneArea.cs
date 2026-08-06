using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArea : MonoBehaviour
{
    private Archer _acher = null;
    private Cannon _cannon = null;
    private Mage _mage = null;
    private Lighting _light = null;
    void Start()
    {
        if (transform.parent.GetComponent<Archer>() != null)
        {
            _acher = transform.parent.GetComponent<Archer>();
        }
        else if (transform.parent.GetComponent<Cannon>() != null)
        {
            _cannon = transform.parent.GetComponent<Cannon>();
        }
        else if(transform.parent.GetComponent<Mage>()!=null)
        {
            _mage = transform.parent.GetComponent<Mage>();
        }
        else
        {
            _light = transform.parent.GetComponent<Lighting>();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && col.GetComponent<EnemyController>()!=null)
        {
            if (_acher != null )
            {
                if (col.GetComponent<EnemyController>().currentHeal > 0)
                    _acher.AddEnemy(col.gameObject);
                else
                    _acher.RemoveEnemy(col.gameObject);
            }
            else if (_cannon != null )
            {
                if (col.GetComponent<EnemyController>().currentHeal > 0)
                    _cannon.AddEnemy(col.gameObject);
                else
                    _cannon.RemoveEnemy(col.gameObject);
            }
            else if(_mage!=null )
            {
                if (col.GetComponent<EnemyController>().currentHeal > 0)
                    _mage.AddEnemy(col.gameObject);
                else
                    _mage.RemoveEnemy(col.gameObject);
            }
            else if(_light!=null )
            {
                if (col.GetComponent<EnemyController>().currentHeal > 0)
                    _light.AddEnemy(col.gameObject);
                else
                    _light.RemoveEnemy(col.gameObject);
            }
        }
        else if (col.gameObject.tag == "Fly")
        {
            if (_acher != null)
            {
                _acher.AddEnemy(col.gameObject);
            }
            else if (_mage != null)
            {
                _mage.AddEnemy(col.gameObject);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (_acher != null)
            {
                _acher.RemoveEnemy(col.gameObject);
            }
            else if (_cannon != null)
            {
                _cannon.RemoveEnemy(col.gameObject);
            }
            else if(_mage!=null)
            {
                _mage.RemoveEnemy(col.gameObject);
            }
            else
            {
                _light.RemoveEnemy(col.gameObject);
            }
        }
        else if (col.gameObject.tag == "Fly")
        {
            if (_acher != null)
            {
                _acher.RemoveEnemy(col.gameObject);
            }
            else if (_mage != null)
            {
                _mage.RemoveEnemy(col.gameObject);
            }
        }
    }
}
