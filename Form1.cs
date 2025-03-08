using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace LocalStickyNotes
{
    public partial class Form1 : Form
    {
        private string noteFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sticky_note.txt");
        private string settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sticky_notes_settings.json");
        private bool isDragging = false;
        private int mouseX, mouseY;
        private Stack<string> undoStack = new Stack<string>(); // Store previous states for undo
        private Stack<string> redoStack = new Stack<string>(); // Store undone states for redo
        private bool isProgrammaticChange = false; // Flag to prevent recursive TextChanged events
        private string lastSavedState = ""; // Track the last saved state
        private bool isPinned = true; // Track pin state

        // Settings class to save form size, position, and font size
        private class AppSettings
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int Left { get; set; }
            public int Top { get; set; }
            public float FontSize { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            LoadSettings(); // Load size, position, and font size
            LoadNote();
            this.TopMost = true; // Always on top by default
            this.BackColor = System.Drawing.Color.LightYellow; // Form background
            titleBarPanel.BackColor = System.Drawing.Color.DarkGray; // Title bar color
            titleBarPanel.Height = 30; // Fixed title bar height
            this.Opacity = 1.0; // Default opacity
            this.FormBorderStyle = FormBorderStyle.None; // Ensure borderless

            // Make the title bar draggable
            titleBarPanel.MouseDown += TitleBarPanel_MouseDown;
            titleBarPanel.MouseMove += TitleBarPanel_MouseMove;
            titleBarPanel.MouseUp += TitleBarPanel_MouseUp;

            // Hook up the TextChanged event
            txtNote.TextChanged += TxtNote_TextChanged;

            // Enable scrolling in TextBox, disable word wrap
            txtNote.ScrollBars = ScrollBars.Vertical;
            txtNote.Multiline = true; // Ensure multiline is enabled for scrolling
            txtNote.WordWrap = false; // Disable automatic wrapping

            // Optional: Hook up the close button
            if (btnClose != null)
            {
                btnClose.Click += (s, e) => this.Close();
            }

            // Hook up the pin button
            if (btnPin != null)
            {
                btnPin.Click += BtnPin_Click;
                btnPin.Text = "📌"; // Pin icon
            }

            // Enable key handling for Ctrl+Z, Ctrl+Y, Ctrl+T, Ctrl+- and Ctrl+=
            this.KeyPreview = true; // Allow the form to capture key events before controls
            this.KeyDown += Form1_KeyDown;

            // Push initial state to undo stack
            undoStack.Push(txtNote.Text);
            lastSavedState = txtNote.Text; // Initialize last saved state
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                if (settings != null)
                {
                    this.Width = 284;
                    this.Height = 461;
                    txtNote.Font = new System.Drawing.Font(txtNote.Font.Name, settings.FontSize);
                }
            }
            else
            {
                // Default values if no settings file exists
                this.Width = 284;
                this.Height = 461;
                txtNote.Font = new System.Drawing.Font(txtNote.Font.Name, 10f);
            }
        }

        private void SaveSettings()
        {
            var settings = new AppSettings
            {
                FontSize = txtNote.Font.Size
            };
            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(settingsFilePath, json);
        }

        private void LoadNote()
        {
            if (File.Exists(noteFilePath))
            {
                txtNote.Text = File.ReadAllText(noteFilePath);
                txtNote.SelectionStart = 0; // Prevent highlighting
                txtNote.SelectionLength = 0;
            }
        }

        private void SaveNote()
        {
            File.WriteAllText(noteFilePath, txtNote.Text);
        }

        private void TxtNote_TextChanged(object sender, EventArgs e)
        {
            // Avoid recursive calls when we programmatically change the text
            if (isProgrammaticChange) return;

            // Only push to undo stack if the text has changed
            if (txtNote.Text != lastSavedState)
            {
                undoStack.Push(txtNote.Text);
                redoStack.Clear(); // Clear redo stack on new change
                lastSavedState = txtNote.Text; // Update last saved state
                SaveNote(); // Auto-save
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle Ctrl+Z for Undo
            if (e.Control && e.KeyCode == Keys.Z)
            {
                e.SuppressKeyPress = true; // Prevent default TextBox undo behavior
                Undo();
            }
            // Handle Ctrl+Y for Redo
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                e.SuppressKeyPress = true; // Prevent default behavior
                Redo();
            }
            // Handle Ctrl+T for Transparency Cycle
            else if (e.Control && e.KeyCode == Keys.T)
            {
                e.SuppressKeyPress = true;
                CycleTransparency();
            }
            // Handle Ctrl+= (increase font size)
            else if (e.Control && (e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add))
            {
                e.SuppressKeyPress = true;
                ChangeFontSize(1f); // Increase by 1
            }
            // Handle Ctrl+- (decrease font size)
            else if (e.Control && (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract))
            {
                e.SuppressKeyPress = true;
                ChangeFontSize(-1f); // Decrease by 1
            }
        }

        private void CycleTransparency()
        {
            // Cycle through opacity levels (e.g., 100%, 75%, 50%, 25%)
            double[] opacityLevels = { 1.0, 0.75, 0.5, 0.25 };
            double currentOpacity = this.Opacity;
            int currentIndex = Array.IndexOf(opacityLevels, currentOpacity);
            int nextIndex = (currentIndex + 1) % opacityLevels.Length;
            this.Opacity = opacityLevels[nextIndex];
        }

        private void ChangeFontSize(float delta)
        {
            float newSize = txtNote.Font.Size + delta;
            if (newSize >= 6f && newSize <= 72f) // Reasonable font size limits
            {
                txtNote.Font = new System.Drawing.Font(txtNote.Font.Name, newSize);
                SaveSettings(); // Save new font size
            }
        }

        private void BtnPin_Click(object sender, EventArgs e)
        {
            isPinned = !isPinned; // Toggle pin state
            this.TopMost = isPinned; // Update TopMost property
            btnPin.Text = isPinned ? "📌" : "📍"; // Update button text
        }

        private void Undo()
        {
            if (undoStack.Count <= 1) return; // Keep at least one state (initial state)

            // Pop the current state and move it to the redo stack
            string currentState = undoStack.Pop();
            redoStack.Push(currentState);

            // Set the TextBox to the previous state
            isProgrammaticChange = true; // Prevent TextChanged from firing
            txtNote.Text = undoStack.Peek();
            txtNote.SelectionStart = txtNote.Text.Length; // Move cursor to end
            txtNote.SelectionLength = 0;
            isProgrammaticChange = false;

            lastSavedState = txtNote.Text; // Update last saved state
            SaveNote(); // Save the undone state
        }

        private void Redo()
        {
            if (redoStack.Count == 0) return; // Nothing to redo

            // Pop the redo state and move it to the undo stack
            string redoState = redoStack.Pop();
            undoStack.Push(redoState);

            // Set the TextBox to the redo state
            isProgrammaticChange = true; // Prevent TextChanged from firing
            txtNote.Text = redoState;
            txtNote.SelectionStart = txtNote.Text.Length; // Move cursor to end
            txtNote.SelectionLength = 0;
            isProgrammaticChange = false;

            lastSavedState = txtNote.Text; // Update last saved state
            SaveNote(); // Save the redone state
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveNote(); // Save note content
            SaveSettings(); // Save size, position, and font size
        }

        // Dragging logic for the title bar
        private void TitleBarPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                mouseX = e.X;
                mouseY = e.Y;
            }
        }

        private void TitleBarPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Left += e.X - mouseX;
                this.Top += e.Y - mouseY;
            }
        }

        private void TitleBarPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }
}