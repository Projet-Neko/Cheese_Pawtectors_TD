using UnityEngine;

public abstract class State
{
    protected Brain _brain;

    public virtual void OnEnter(Brain brain)
    {
        _brain = brain;

        if (_brain.Entity is Cat) Mod_Waves.OnWaveReload += M_Wave_OnWaveReload;

        if (_brain.Entity is Cat) Debug.Log($"New state for {_brain.Entity.name} : {this}");
    }

    public virtual void OnUpdate() { }

    public virtual void OnExit()
    {
        if (_brain.Entity is Cat) Mod_Waves.OnWaveReload -= M_Wave_OnWaveReload;
    }

    protected bool IsInFollowRange()
    {
        if (_brain.Entity is not Cat) return false;

        Collider2D[] targets = Physics2D.OverlapCircleAll(_brain.transform.position, _brain.FollowRange);

        foreach (Collider2D target in targets)
        {
            // layer "Enemy" (Mouse)
            if (target.gameObject.layer == 8)
            {
                //if (!_brain.Room.bounds.Contains(target.transform.position))
                //{
                //    Debug.Log($"{target.gameObject.name} is not in the same room");
                //    Debug.Log(_brain.Room.bounds);
                //    Debug.Log(target.transform.position);
                //    continue;
                //}

                Mouse m = target.GetComponentInParent<Mouse>();

                if ((m.IsBoss || m.Attacker == null) && _brain.Target == null)
                {
                    m.Attacker = _brain.Entity as Cat;
                    _brain.Target = target.gameObject;
                    Debug.Log($"<color=red>{_brain.Entity.name} is targeting {m.name}</color>");
                    return true;
                }
                else if (m.Attacker == _brain.Entity as Cat) return true;
            }
        }

        _brain.Target = null;
        return false;
    }

    protected bool IsInAttackRange()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(_brain.transform.position, _brain.AttackRange);

        foreach (Collider2D target in targets)
        {
            //if (_brain.Entity is Cat && !_brain.Room.bounds.Contains(target.transform.position)) continue;
            if (target.gameObject == _brain.Target)
            {
                Debug.Log($"{_brain.Entity.name} is attacking {target.gameObject.name}");
                return true;
            }
        }

        return false;
    }

    protected void FollowTarget()
    {
       
        /*_brain.transform.position = Vector3.MoveTowards(_brain.transform.position, _brain.Target.transform.position, _brain.Entity.Speed * Time.deltaTime);

        if (_brain.Entity is not Cat) return;

        // Acc�der au tableau de sprites du chat
        Sprite[] catSprites = (_brain.Entity as Cat).Sprites;


        // Calculer la direction du mouvement
        Vector3 moveDirection = (_brain.Target.transform.position - _brain.transform.position).normalized;

        // D�terminer le secteur dans lequel se trouve le mouvement
        int sector = GetMovementSector(moveDirection);

        // Choisir le sprite en fonction du secteur
        _brain.Entity.Renderer.sprite = catSprites[sector];*/
    }

    // M�thode pour d�terminer le secteur du mouvement
    private int GetMovementSector(Vector3 moveDirection)
    {
        // Convertir le vecteur de direction en angle
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // Ajouter 360 degr�s pour �viter les angles n�gatifs
        if (angle < 0)
        {
            angle += 360;
        }

        // Diviser le cercle en 8 secteurs et assigner un secteur pour chaque direction
        if (angle >= 22.5f && angle < 67.5f)
        {
            return 2; // Nord-Est
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            return 1; // Nord
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            return 3; // Nord-Ouest
        }
        else if (angle >= 157.5f && angle < 202.5f)
        {
            return 7; // Ouest
        }
        else if (angle >= 202.5f && angle < 247.5f)
        {
            return 6; // Sud-Ouest
        }
        else if (angle >= 247.5f && angle < 292.5f)
        {
            return 4; // Sud
        }
        else if (angle >= 292.5f && angle < 337.5f)
        {
            return 5; // Sud-Est
        }
        else
        {
            return 0; // Est
        }
    }

    protected void M_Wave_OnWaveReload()
    {
        if (_brain.Entity is not Cat) return;
        (_brain.Entity as Cat).WakeUp();
        _brain.ChangeState(_brain.Idle);
    }
}
