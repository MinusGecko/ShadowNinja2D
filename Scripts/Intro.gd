extends Control

const GAME_SCENE := "res://Scenes/Main.tscn"
var pages := [
    "ERYNDOR...\nUn mundo donde antiguos castillos conviven con máquinas alimentadas por una energía llamada el Pulso.",
    "Pero un experimento salió mal.\nLa Plaga del Vacío comenzó a consumir aldeas, criaturas y recuerdos.",
    "KAIRO tenía una vida sencilla. En la academia conoció a AYLA.\nCompartieron conversaciones, risas y momentos que él guardaría para siempre.",
    "Un día, sus caminos se separaron. Kairo se quedó con una pequeña liga para el cabello y muchas palabras que nunca llegó a decir.",
    "Entonces, una fuerza enmascarada se llevó a Ayla hacia la Ciudadela del Eclipse.\nLa Plaga también marcó a Kairo y el tiempo empezó a correr en su contra.",
    "Kairo decidió emprender el viaje. No sabe qué sentirá Ayla al verlo, ni espera que ella le deba una respuesta.\nSolo quiere encontrarla y decirle lo que lleva guardado."
]
var page_index := 0

var story_label: Label
var page_label: Label
var continue_button: Button

func _ready() -> void:
    _build_intro()
    _show_page()

func _build_intro() -> void:
    var background := TextureRect.new()
    background.texture = load("res://Art/Backgrounds/ForestRuins.svg")
    background.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
    background.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
    background.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_COVERED
    add_child(background)

    var shade := ColorRect.new()
    shade.color = Color(0.01, 0.02, 0.045, 0.78)
    shade.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
    add_child(shade)

    var layout := VBoxContainer.new()
    layout.anchor_left = 0.12
    layout.anchor_top = 0.16
    layout.anchor_right = 0.88
    layout.anchor_bottom = 0.86
    layout.add_theme_constant_override("separation", 28)
    add_child(layout)

    var chapter := Label.new()
    chapter.text = "PRÓLOGO  •  EL COMIENZO DEL VIAJE"
    chapter.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
    chapter.add_theme_font_size_override("font_size", 22)
    chapter.add_theme_color_override("font_color", Color(0.5, 0.82, 0.92))
    layout.add_child(chapter)

    var spacer := Control.new()
    spacer.size_flags_vertical = Control.SIZE_EXPAND_FILL
    layout.add_child(spacer)

    story_label = Label.new()
    story_label.custom_minimum_size = Vector2(0, 220)
    story_label.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
    story_label.vertical_alignment = VERTICAL_ALIGNMENT_CENTER
    story_label.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
    story_label.add_theme_font_size_override("font_size", 28)
    story_label.add_theme_color_override("font_color", Color(0.94, 0.96, 1))
    layout.add_child(story_label)

    page_label = Label.new()
    page_label.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
    page_label.add_theme_font_size_override("font_size", 16)
    page_label.add_theme_color_override("font_color", Color(0.58, 0.72, 0.82))
    layout.add_child(page_label)

    var spacer2 := Control.new()
    spacer2.size_flags_vertical = Control.SIZE_EXPAND_FILL
    layout.add_child(spacer2)

    continue_button = Button.new()
    continue_button.custom_minimum_size = Vector2(280, 58)
    continue_button.size_flags_horizontal = Control.SIZE_SHRINK_CENTER
    continue_button.add_theme_font_size_override("font_size", 22)
    continue_button.pressed.connect(_next_page)
    layout.add_child(continue_button)

func _show_page() -> void:
    story_label.text = pages[page_index]
    page_label.text = "%d / %d" % [page_index + 1, pages.size()]
    continue_button.text = "CONTINUAR" if page_index < pages.size() - 1 else "COMENZAR AVENTURA"

func _next_page() -> void:
    page_index += 1
    if page_index >= pages.size():
        get_tree().change_scene_to_file(GAME_SCENE)
    else:
        _show_page()

func _unhandled_input(event: InputEvent) -> void:
    if event.is_action_pressed("ui_accept"):
        _next_page()
