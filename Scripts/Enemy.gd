extends StaticBody2D

@export var max_health: int = 2
var health: int

@onready var visual: Polygon2D = $Visual

func _ready() -> void:
    health = max_health

func take_damage(amount: int = 1) -> void:
    health = maxi(0, health - amount)
    visual.modulate = Color(1.0, 0.65, 0.65, 1.0)
    if health <= 0:
        queue_free()
    else:
        await get_tree().create_timer(0.12).timeout
        if is_instance_valid(visual):
            visual.modulate = Color.WHITE
