extends Area2D

@export_enum("energy", "memory") var item_type: String = "energy"
@export var amount: int = 1
var phase: float = 0.0

@onready var visual: Polygon2D = $Visual

func _ready() -> void:
    body_entered.connect(_on_body_entered)

func _process(delta: float) -> void:
    phase += delta
    visual.rotation += delta * 1.1
    visual.position.y = sin(phase * 2.6) * 5.0

func _on_body_entered(body: Node2D) -> void:
    if not body.is_in_group("player"):
        return
    if item_type == "memory":
        body.collect_memory()
    else:
        body.collect_energy(amount)
    queue_free()
