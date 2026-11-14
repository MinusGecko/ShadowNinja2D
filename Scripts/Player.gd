extends CharacterBody2D

@export var move_speed: float = 320.0
@export var jump_velocity: float = -570.0
@export var gravity: float = 1500.0
@export var max_health: int = 5

var health: int
var energy_shards: int = 0
var memories_found: int = 0
var facing: int = 1
var attack_timer: float = 0.0
var attack_cooldown: float = 0.0
var invulnerability_timer: float = 0.0
var anim_time: float = 0.0
var attack_anim_time: float = 0.0
var visual_base_position := Vector2(0, -11)
var visual_base_scale := Vector2(0.48, 0.48)

@onready var attack_area: Area2D = $AttackArea
@onready var attack_shape: CollisionShape2D = $AttackArea/CollisionShape2D
@onready var body_visual: Sprite2D = $BodyVisual
@onready var attack_flash: Polygon2D = $AttackFlash

func _ready() -> void:
    health = max_health
    add_to_group("player")
    _update_status()
    body_visual.position = visual_base_position
    body_visual.scale = visual_base_scale
    attack_area.monitoring = false
    attack_shape.disabled = true
    attack_flash.visible = false
    attack_area.body_entered.connect(_on_attack_area_body_entered)

func _physics_process(delta: float) -> void:
    anim_time += delta
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
        attack_anim_time += delta
        if attack_timer <= 0.0:
            attack_area.monitoring = false
            attack_shape.set_deferred("disabled", true)
            attack_flash.visible = false
            attack_anim_time = 0.0

    _animate_kairo(direction)

    if invulnerability_timer > 0.0:
        body_visual.modulate = Color(1.0, 0.45, 0.45, 1.0) if int(invulnerability_timer * 12.0) % 2 == 0 else Color.WHITE
    else:
        body_visual.modulate = Color.WHITE

    move_and_slide()

func _animate_kairo(direction: float) -> void:
    var target_position := visual_base_position
    var target_rotation := 0.0
    var target_scale := visual_base_scale

    if attack_timer > 0.0:
        var swing := sin((attack_anim_time / 0.16) * PI)
        target_rotation = -0.32 * float(facing) * swing
        target_position.x += 5.0 * float(facing) * swing
        target_scale = visual_base_scale * Vector2(1.12, 0.92)
        attack_flash.rotation = -0.25 * float(facing)
        attack_flash.scale = Vector2(0.65 + swing * 0.55, 0.8 + swing * 0.3)
    elif not is_on_floor():
        target_rotation = clampf(-velocity.y / 1800.0, -0.22, 0.22) * float(facing)
        target_position.y -= 2.0
        target_scale = visual_base_scale * Vector2(0.94, 1.08)
    elif absf(direction) > 0.1:
        var run_phase := anim_time * 13.0
        target_position.y += absf(sin(run_phase)) * 5.0
        target_rotation = sin(run_phase) * 0.055 * float(facing)
        target_scale = visual_base_scale * Vector2(1.0 + absf(sin(run_phase)) * 0.035, 1.0 - absf(sin(run_phase)) * 0.035)
    else:
        target_position.y += sin(anim_time * 2.8) * 2.0
        target_rotation = sin(anim_time * 1.4) * 0.018

    body_visual.position = body_visual.position.lerp(target_position, 0.28)
    body_visual.rotation = lerp_angle(body_visual.rotation, target_rotation, 0.3)
    body_visual.scale = body_visual.scale.lerp(target_scale, 0.3)

func _attack() -> void:
    if attack_cooldown > 0.0:
        return
    attack_cooldown = 0.38
    attack_timer = 0.16
    attack_anim_time = 0.0
    attack_area.position.x = 38.0 * facing
    attack_area.monitoring = true
    attack_shape.set_deferred("disabled", false)
    attack_flash.position.x = 38.0 * facing
    attack_flash.scale.x = float(facing)
    attack_flash.visible = true

func take_damage(amount: int = 1) -> void:
    if invulnerability_timer > 0.0:
        return
    health = maxi(0, health - amount)
    _update_status()
    invulnerability_timer = 0.8
    velocity.y = -220.0
    velocity.x = -facing * 180.0
    if health <= 0:
        get_tree().reload_current_scene()

func _on_attack_area_body_entered(body: Node2D) -> void:
    if body.has_method("take_damage"):
        body.take_damage(1)

func collect_energy(value: int = 1) -> void:
    energy_shards += value
    _update_status()

func collect_memory() -> void:
    memories_found += 1
    _update_status()

func _update_status() -> void:
    var status_nodes := get_tree().get_nodes_in_group("hud_status")
    for status in status_nodes:
        status.text = "VIDA: %d   ENERGÍA: %d   RECUERDOS: %d/3" % [health, energy_shards, memories_found]
