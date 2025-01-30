using UnityEditor.Animations;
using UnityEngine;

public class TransformationBase_SO : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public GameObject prefab;
    public Avatar avatar;
    public AnimatorController animatorController;

    [Header("Physical stats")]
    public float walkingspeed;
    public float runningSpeed;
    public float move_turning_speed;
    public float idle_turning_speed;
    public float jumpPower;
    public float fallMultiplier;
    public float air_control_amount;
    public float jump_cooldown;

    public void CopyFrom(TransformationBase_SO source)
    {
        if (source == null) return;

        name = source.name;
        prefab = source.prefab;

        walkingspeed = source.walkingspeed;
        runningSpeed = source.runningSpeed;
        move_turning_speed = source.move_turning_speed;
        idle_turning_speed = source.idle_turning_speed;
        jumpPower = source.jumpPower;
        air_control_amount = source.air_control_amount;
        jump_cooldown = source.jump_cooldown;
    }


}
