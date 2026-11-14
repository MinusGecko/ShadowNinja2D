extends Area2D

@export_file("*.tscn") var next_scene: String
@export var prompt_text: String = "ESCENA COMPLETADA"
var activated := false

func _ready() -> void:
    body_entered.connect(_on_body_entered)

func _on_body_entered(body: Node2D) -> void:
    if activated or not body.is_in_group("player"):
        return
    activated = true
    var label := Label.new()
    label.text = prompt_text
    label.add_theme_font_size_override("font_size", 28)
    label.add_theme_color_override("font_color", Color(0.75, 0.95, 1))
    label.position = Vector2(-170, -100)
    add_child(label)
    await get_tree().create_timer(0.8).timeout
    if ResourceLoader.exists(next_scene):
        get_tree().change_scene_to_file(next_scene)
    else:
        activated = false
