#  Hierarchy Auto Organizer for Unity

**Hierarchy Auto Organizer** is a powerful and easy-to-use Unity Editor extension that cleans and organizes your scene hierarchy automatically. Ideal for both large production projects and small indie teams, this tool streamlines workflows by reducing clutter and improving readability in your Unity scenes.

---

## ✨ Features

- 🔹 **Create customizable Marker GameObjects with editable names and colors to visually separate sections in hierarchy**
- 🔹 **Auto-detect and group Environment objects**
- 🔹 **Organize UI Canvases into a UI Root**
- 🔹 **Detect & group commonly used empty Transforms and Manager objects**
- 🔹 **Custom grouping by Name or Tag**
- 🔹 **Exclude specific GameObject tags from grouping**
- 🔹 **Undo support for all hierarchy changes**
- 🔹 **Smart deletion of unused empty GameObjects**
- 🔹 **Clean, responsive Editor window with tooltips and grouping options**

---

## 🛠 Installation

1. Clone or download this repository.
2. Copy the `HierarchyAutoOrganizer` folder into your Unity project's `Assets/Editor/` directory.
3. Open Unity.
4. Go to **Tools > Hierarchy Organizer** from the top menu bar.

> ✅ This tool is Editor-only. It does not affect builds or runtime performance.

---

## 🚀 Usage

### 🧱 Organizers

| Button | Action |
|--------|--------|
| `Remove Unuseful GameObjects` | Deletes all empty GameObjects that aren't referenced anywhere. |
| `Auto Arrange Useful Empty GameObjects` | Groups useful empty Transforms and Manager scripts under labeled roots. |
| `Auto Arrange UI Elements` | Organizes Canvas objects under a single UI root. |
| `Auto Arrange 3D Environment` | Moves environment meshes under a labeled Environment root. |
| `Auto Arrange Environment Children` | Optionally group them by name or tag into parent folders. |

---

### 🧩 Grouping Options

You can configure grouping behavior via the **Environment Grouping Options** section:

- **Group By**:  
  - `Name`: Groups by base name (e.g. `Tree_01`, `Tree_02` → `Tree`)
  - `Tag`: Groups objects by tag (e.g. all `P rops` in one folder)

- **Exclude Tags**: Add tags like `Player`, `Interactable` to prevent them from being grouped.

---

## 🧠 Best Practices

- Use `Generate Empty Marker` to create labeled separator objects in the hierarchy.
- Always commit your scene before using bulk auto-organization for safety.
- This tool is designed to be **non-destructive** and uses **Undo** for all actions.
