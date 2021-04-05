# ripl Visual Studio Extensions

These are a set of Visual Studio Extensions that I created for myself while writing my book [Get Going With F#](https://github.com/rchandrashekara/get-going-with-fsharp). All commands are accessed through the _Extensions > ripl_ menu. The following menu items are implemented:

1. **Copy with Line Numbers:** Copies the selected text with line numbers. The copied text is placed in the clipboard in two data formats, plain text and also RTF. The command works best with F# but other languages should also be supported as many languages share common keywords like if, for, then, do, etc. ...

![Scrivener Editing Window](https://github.com/rchandrashekara/riplVisualStudioExtensions/blob/main/LineNumbers.jpg "Scrivener Editing Window")

2. **Add Comment:** Adds a comment to the selected text. The code is essentially the same code that you can create for yourself by following the instructions at [Walkthrough: Using a Shell Command with an Editor Extension](https://docs.microsoft.com/en-gb/previous-versions/visualstudio/visual-studio-2015/extensibility/walkthrough-using-a-shell-command-with-an-editor-extension?view=vs-2015&redirectedfrom=MSDN). I found the link to this walkthrough through this [link](https://stackoverflow.com/questions/2868127/get-the-selected-text-of-the-editor-window-visual-studio-extension).
