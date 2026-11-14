extends StaticBody2D

@export var max_health: int = 2
var health: int
var anim_time: float = 0.0
var base_position := Vector2(0, -11)

@onready var visual: Sprite2D = $Visual

func _ready() -> void:
    health = max_health
    base_position = visual.position

func _process(delta: float) -> void:
    anim_time += delta
    visual.position.y = base_position.y + sin(anim_time * 2.4) * 2.5
    visual.rotation = sin(anim_time * 1.6) * 0.025
    var pulse := 1.0 + sin(anim_time * 3.0) * 0.018
    visual.scale = Vector2(0.48, 0.48) * pulse

func take_damage(amount: int = 1) -> void:
    health = maxi(0, health - amount)
    visual.modulate = Color(1.0, 0.55, 0.5, 1.0)
    if health <= 0:
        queue_free()
    else:
        await get_tree().create_timer(0.12).timeout
        if is_instance_valid(visual):
            visual.modulate = Color.WHITE
