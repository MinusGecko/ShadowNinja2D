extends CharacterBody2D

@export var move_speed: float = 300.0
@export var jump_velocity: float = -560.0
@export var gravity: float = 1500.0

var facing: int = 1
var attacking: bool = false
var attack_time: float = 0.0

func _physics_process(delta: float) -> void:
    if not is_on_floor():
        velocity.y += gravity * delta
    elif Input.is_action_just_pressed("jump"):
        velocity.y = jump_velocity

    var direction := Input.get_axis("move_left", "move_right")
    if direction != 0.0:
        facing = 1 if direction > 0.0 else -1
    velocity.x = direction * move_speed

    if Input.is_action_just_pressed("attack"):
        _attack()

    if attacking:
        attack_time -= delta
        if attack_time <= 0.0:
            attacking = false

    move_and_slide()

func _attack() -> void:
    if attacking:
        return
    attacking = true
    attack_time = 0.22
    print("Kairo attacks toward ", "right" if facing > 0 else "left")
