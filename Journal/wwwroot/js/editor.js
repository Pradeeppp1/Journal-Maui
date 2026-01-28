let quill;
const turndownService = new TurndownService();

window.initializeQuill = (elementId, initialContent) => {
    quill = new Quill(`#${elementId}`, {
        theme: 'snow',
        placeholder: 'Write your thoughts here...',
        modules: {
            toolbar: [
                [{ 'header': [1, 2, 3, false] }],
                ['bold', 'italic', 'underline', 'strike'],
                ['blockquote', 'code-block'],
                [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                ['link'],
                ['clean']
            ]
        }
    });

    if (initialContent) {
        quill.root.innerHTML = initialContent;
    }

    // Set height
    quill.root.style.minHeight = '300px';

    return true;
};

window.getQuillContent = () => {
    return quill.root.innerHTML;
};

window.getQuillMarkdown = () => {
    const html = quill.root.innerHTML;
    return turndownService.turndown(html);
};

window.setQuillContent = (content) => {
    if (quill) {
        quill.root.innerHTML = content;
    }
};

window.onQuillContentChanged = (dotNetReference) => {
    quill.on('text-change', () => {
        dotNetReference.invokeMethodAsync('OnEditorContentChanged', quill.root.innerHTML);
    });
};
