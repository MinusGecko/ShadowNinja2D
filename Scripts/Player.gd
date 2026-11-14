extends CharacterBody2D

@export var move_speed: float = 320.0
@export var jump_velocity: float = -570.0
@export var gravity: float = 1500.0
@export var max_health: int = 5

var health: int
var facing: int = 1
var attack_timer: float = 0.0
var attack_cooldown: float = 0.0
var invulnerability_timer: float = 0.0

@onready var attack_area: Area2D = $AttackArea
@onready var attack_shape: CollisionShape2D = $AttackArea/CollisionShape2D
@onready var body_visual: Sprite2D = $BodyVisual
@onready var attack_flash: Polygon2D = $AttackFlash

func _ready() -> void:
    health = max_health
    attack_area.monitoring = false
    attack_shape.disabled = true
    attack_flash.visible = false
    attack_area.body_entered.connect(_on_attack_area_body_entered)

func _physics_process(delta: float) -> void:
    attack_cooldown = maxf(0.0, attack_cooldown - delta)
    invulnerability_timer = maxf(0.0, invulnerability_timer - delta)

    if not is_on_floor():
        velocity.y += gravity * delta
    elif Input.is_action_just_pressed("jump"):
        velocity.y = jump_velocity

    var direction := Input.get_axis("move_left", "move_right")
    if direction != 0.0:
        facing = 1 if direction > 0.0 else -1
        body_visual.flip_h = facing < 0
    velocity.x = direction * move_speed

    if Input.is_action_just_pressed("attack"):
        _attack()

    if attack_timer > 0.0:
        attack_timer -= delta
        if attack_timer <= 0.0:
            attack_area.monitoring = false
            attack_shape.set_deferred("disabled", true)
            attack_flash.visible = false

    if invulnerability_timer > 0.0:
        body_visual.modulate = Color(1.0, 0.45, 0.45, 1.0) if int(invulnerability_timer * 12.0) % 2 == 0 else Color.WHITE
    else:
        body_visual.modulate = Color.WHITE

    move_and_slide()

func _attack() -> void:
    if attack_cooldown > 0.0:
        return
    attack_cooldown = 0.38
    attack_timer = 0.16
    attack_area.position.x = 38.0 * facing
    attack_area.monitoring = true
    attack_shape.set_deferred("disabled", false)
    attack_flash.position.x = 38.0 * facing
    attack_flash.scale.x = facing
    attack_flash.visible = true

func take_damage(amount: int = 1) -> void:
    if invulnerability_timer > 0.0:
        return
    health = maxi(0, health - amount)
    invulnerability_timer = 0.8
    velocity.y = -220.0
    velocity.x = -facing * 180.0
    if health <= 0:
        get_tree().reload_current_scene()

func _on_attack_area_body_entered(body: Node2D) -> void:
    if body.has_method("take_damage"):
        body.take_damage(1)
