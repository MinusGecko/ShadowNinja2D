extends Control

func _ready() -> void:
    if not OS.has_feature("android") and not OS.has_feature("ios") and not DisplayServer.is_touchscreen_available():
        return
    _make_button("◀", "move_left", Vector2(24, -112), Vector2(76, 76))
    _make_button("▶", "move_right", Vector2(112, -112), Vector2(76, 76))
    _make_button("SALTO", "jump", Vector2(-190, -112), Vector2(86, 76), true)
    _make_button("ATAQUE", "attack", Vector2(-96, -112), Vector2(86, 76), true)

func _make_button(label_text: String, action: String, offset: Vector2, button_size: Vector2, from_right: bool = false) -> void:
    var button := Button.new()
    button.text = label_text
    button.custom_minimum_size = button_size
    button.add_theme_font_size_override("font_size", 16)
    button.modulate = Color(0.82, 0.9, 1.0, 0.88)
    button.focus_mode = Control.FOCUS_NONE
    add_child(button)
    if from_right:
        button.set_anchors_and_offsets_preset(Control.PRESET_BOTTOM_RIGHT)
    else:
        button.set_anchors_and_offsets_preset(Control.PRESET_BOTTOM_LEFT)
    button.position = offset
    button.button_down.connect(func(): Input.action_press(action))
    button.button_up.connect(func(): Input.action_release(action))
    button.tree_exiting.connect(func(): Input.action_release(action))
