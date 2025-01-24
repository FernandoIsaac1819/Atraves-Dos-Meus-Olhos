using UnityEngine;

public class TransformationBase_SO : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public GameObject form_prefab;
    public Avatar form_avatar;
    public CapsuleCollider form_collider;
    public float duration;

    [Header("Physical stats")]
    public float walking_speed;
    public float running_speed;
    public float move_turning_speed;
    public float idle_turning_speed;
    public float jump_power;
    public float air_control_amount;
    public float jump_cooldown;

    public void CopyFrom(TransformationBase_SO source)
    {
        if (source == null) return;

        name = source.name;
        form_prefab = source.form_prefab;
        form_avatar = source.form_avatar;
        form_collider = source.form_collider;
        duration = source.duration;

        walking_speed = source.walking_speed;
        running_speed = source.running_speed;
        move_turning_speed = source.move_turning_speed;
        idle_turning_speed = source.idle_turning_speed;
        jump_power = source.jump_power;
        air_control_amount = source.air_control_amount;
        jump_cooldown = source.jump_cooldown;
    }
}
