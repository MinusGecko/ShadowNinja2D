extends Control

const INTRO_SCENE := "res://Scenes/Intro.tscn"

func _ready() -> void:
    _build_menu()

func _build_menu() -> void:
    var background := TextureRect.new()
    background.texture = load("res://Art/Backgrounds/ForestRuins.svg")
    background.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
    background.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
    background.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_COVERED
    add_child(background)

    var shade := ColorRect.new()
    shade.color = Color(0.015, 0.025, 0.055, 0.58)
    shade.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
    add_child(shade)

    var panel := VBoxContainer.new()
    panel.anchor_left = 0.5
    panel.anchor_top = 0.5
    panel.anchor_right = 0.5
    panel.anchor_bottom = 0.5
    panel.offset_left = -300
    panel.offset_top = -180
    panel.offset_right = 300
    panel.offset_bottom = 180
    panel.alignment = BoxContainer.ALIGNMENT_CENTER
    panel.add_theme_constant_override("separation", 22)
    add_child(panel)

    var title := Label.new()
    title.text = "SHADOW NINJA 2D"
    title.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
    title.add_theme_font_size_override("font_size", 48)
    title.add_theme_color_override("font_color", Color(0.82, 0.94, 1))
    panel.add_child(title)

    var subtitle := Label.new()
    subtitle.text = "EL ÚLTIMO TE AMO"
    subtitle.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
    subtitle.add_theme_font_size_override("font_size", 24)
    subtitle.add_theme_color_override("font_color", Color(0.55, 0.82, 0.9))
    panel.add_child(subtitle)

    var spacer := Control.new()
    spacer.custom_minimum_size.y = 22
    panel.add_child(spacer)

    var start := Button.new()
    start.text = "INICIAR JUEGO"
    start.custom_minimum_size = Vector2(300, 64)
    start.add_theme_font_size_override("font_size", 26)
    start.pressed.connect(_start_game)
    panel.add_child(start)

    var footer := Label.new()
    footer.text = "A/D o flechas: moverse   •   ESPACIO: saltar   •   CLIC: atacar"
    footer.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
    footer.add_theme_font_size_override("font_size", 14)
    footer.add_theme_color_override("font_color", Color(0.78, 0.84, 0.9))
    panel.add_child(footer)

func _start_game() -> void:
    get_tree().change_scene_to_file(INTRO_SCENE)
