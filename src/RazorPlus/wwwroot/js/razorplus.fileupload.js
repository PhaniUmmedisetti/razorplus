/**
 * RazorPlus File Upload Module
 * Drag-and-drop file upload with preview, validation, and progress tracking
 * No external dependencies - pure JavaScript
 */

const uploaders = new WeakMap();

// File utilities
const fileUtils = {
  formatSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  },

  isImage(file) {
    return file.type.startsWith('image/');
  },

  matchesAccept(file, accept) {
    if (!accept) return true;
    const types = accept.split(',').map(t => t.trim().toLowerCase());
    const fileName = file.name.toLowerCase();
    const fileType = file.type.toLowerCase();

    return types.some(type => {
      if (type.startsWith('.')) {
        // Extension match (e.g., ".pdf")
        return fileName.endsWith(type);
      } else if (type.endsWith('/*')) {
        // Wildcard match (e.g., "image/*")
        return fileType.startsWith(type.replace('/*', ''));
      } else {
        // MIME type match (e.g., "application/pdf")
        return fileType === type;
      }
    });
  },

  async generatePreview(file) {
    return new Promise((resolve, reject) => {
      if (!fileUtils.isImage(file)) {
        resolve(null);
        return;
      }

      const reader = new FileReader();
      reader.onload = (e) => resolve(e.target.result);
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
  },

  validateFile(file, maxSize, accept) {
    const errors = [];

    if (maxSize && file.size > maxSize) {
      errors.push(`File size exceeds ${fileUtils.formatSize(maxSize)}`);
    }

    if (accept && !fileUtils.matchesAccept(file, accept)) {
      errors.push(`File type not allowed`);
    }

    return errors;
  }
};

// Build file preview card
function buildFileCard(file, index, preview = null, progress = null) {
  const card = document.createElement('div');
  card.className = 'rp-file-card';
  card.setAttribute('role', 'listitem');
  card.dataset.index = index;

  let html = '<div class="rp-file-card__content">';

  // Preview or icon
  if (preview) {
    html += `<div class="rp-file-card__preview"><img src="${preview}" alt="${file.name}" /></div>`;
  } else {
    const icon = fileUtils.isImage(file) ? '🖼️' : '📄';
    html += `<div class="rp-file-card__icon">${icon}</div>`;
  }

  // File info
  html += '<div class="rp-file-card__info">';
  html += `<div class="rp-file-card__name" title="${file.name}">${file.name}</div>`;
  html += `<div class="rp-file-card__size">${fileUtils.formatSize(file.size)}</div>`;
  html += '</div>';

  // Remove button
  html += `<button type="button" class="rp-file-card__remove" aria-label="Remove file" title="Remove">&times;</button>`;

  html += '</div>';

  // Progress bar (if uploading)
  if (progress !== null) {
    html += '<div class="rp-file-card__progress">';
    html += `<div class="rp-file-card__progress-bar" style="width: ${progress}%"></div>`;
    html += '</div>';
  }

  card.innerHTML = html;
  return card;
}

// Initialize file upload
export function enhanceFileUpload(container) {
  if (container.dataset.rpEnhanced) return;
  container.dataset.rpEnhanced = 'true';

  const dropzone = container.querySelector('.rp-file-upload__dropzone');
  const input = container.querySelector('.rp-file-upload__input');
  const browseBtn = container.querySelector('.rp-file-upload__browse');
  const filesList = container.querySelector('.rp-file-upload__files');

  const maxSize = parseInt(container.dataset.maxSize || '5242880'); // 5MB default
  const maxFiles = parseInt(container.dataset.maxFiles || '10');
  const accept = container.dataset.accept || null;
  const showPreview = container.dataset.preview === 'true';
  const uploadUrl = container.dataset.uploadUrl || null;

  const state = {
    files: [],
    isDragging: false
  };

  uploaders.set(container, state);

  // Browse button click
  browseBtn?.addEventListener('click', (e) => {
    e.preventDefault();
    e.stopPropagation();
    input?.click();
  });

  // Dropzone click
  dropzone?.addEventListener('click', (e) => {
    if (e.target === browseBtn) return;
    input?.click();
  });

  // Keyboard support for dropzone
  dropzone?.addEventListener('keydown', (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      input?.click();
    }
  });

  // File input change
  input?.addEventListener('change', (e) => {
    const files = Array.from(e.target.files || []);
    handleFiles(files);
  });

  // Drag and drop events
  ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropzone?.addEventListener(eventName, preventDefaults, false);
  });

  function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
  }

  ['dragenter', 'dragover'].forEach(eventName => {
    dropzone?.addEventListener(eventName, () => {
      if (!state.isDragging) {
        state.isDragging = true;
        dropzone?.classList.add('rp-file-upload__dropzone--dragging');
      }
    });
  });

  ['dragleave', 'drop'].forEach(eventName => {
    dropzone?.addEventListener(eventName, () => {
      state.isDragging = false;
      dropzone?.classList.remove('rp-file-upload__dropzone--dragging');
    });
  });

  dropzone?.addEventListener('drop', (e) => {
    const dt = e.dataTransfer;
    const files = Array.from(dt?.files || []);
    handleFiles(files);
  });

  // Handle selected files
  async function handleFiles(newFiles) {
    if (!newFiles.length) return;

    // Check max files limit
    const multiple = input?.hasAttribute('multiple');
    if (!multiple && newFiles.length > 1) {
      newFiles = [newFiles[0]];
    }

    const totalFiles = state.files.length + newFiles.length;
    if (totalFiles > maxFiles) {
      alert(`Maximum ${maxFiles} files allowed`);
      return;
    }

    // Validate and add files
    for (const file of newFiles) {
      const errors = fileUtils.validateFile(file, maxSize, accept);
      if (errors.length > 0) {
        alert(`${file.name}: ${errors.join(', ')}`);
        continue;
      }

      const fileData = {
        file,
        preview: null,
        progress: uploadUrl ? 0 : null
      };

      // Generate preview for images
      if (showPreview && fileUtils.isImage(file)) {
        try {
          fileData.preview = await fileUtils.generatePreview(file);
        } catch (err) {
          console.warn('Failed to generate preview', err);
        }
      }

      state.files.push(fileData);

      // Upload if URL provided
      if (uploadUrl) {
        uploadFile(fileData, state.files.length - 1);
      }
    }

    renderFiles();

    // Clear input so same file can be selected again
    if (input) input.value = '';
  }

  // Upload file via AJAX
  async function uploadFile(fileData, index) {
    const formData = new FormData();
    formData.append('file', fileData.file);

    try {
      const xhr = new XMLHttpRequest();

      xhr.upload.addEventListener('progress', (e) => {
        if (e.lengthComputable) {
          const progress = Math.round((e.loaded / e.total) * 100);
          fileData.progress = progress;
          updateFileProgress(index, progress);
        }
      });

      xhr.addEventListener('load', () => {
        if (xhr.status >= 200 && xhr.status < 300) {
          fileData.progress = 100;
          updateFileProgress(index, 100);
          // Dispatch success event
          container.dispatchEvent(new CustomEvent('rp-upload-success', {
            detail: { file: fileData.file, response: xhr.responseText, index }
          }));
        } else {
          fileData.progress = null;
          updateFileProgress(index, null);
          alert(`Upload failed: ${xhr.statusText}`);
        }
      });

      xhr.addEventListener('error', () => {
        fileData.progress = null;
        updateFileProgress(index, null);
        alert('Upload failed due to network error');
      });

      xhr.open('POST', uploadUrl, true);
      xhr.send(formData);
    } catch (err) {
      console.error('Upload error', err);
      fileData.progress = null;
      updateFileProgress(index, null);
    }
  }

  // Update progress bar for a file
  function updateFileProgress(index, progress) {
    const card = filesList?.querySelector(`[data-index="${index}"]`);
    if (!card) return;

    let progressBar = card.querySelector('.rp-file-card__progress');
    if (progress === null) {
      progressBar?.remove();
      return;
    }

    if (!progressBar) {
      progressBar = document.createElement('div');
      progressBar.className = 'rp-file-card__progress';
      progressBar.innerHTML = '<div class="rp-file-card__progress-bar"></div>';
      card.appendChild(progressBar);
    }

    const bar = progressBar.querySelector('.rp-file-card__progress-bar');
    if (bar) {
      bar.style.width = `${progress}%`;
    }
  }

  // Render file list
  function renderFiles() {
    if (!filesList) return;

    filesList.innerHTML = '';

    if (state.files.length === 0) {
      filesList.style.display = 'none';
      return;
    }

    filesList.style.display = 'grid';

    state.files.forEach((fileData, index) => {
      const card = buildFileCard(fileData.file, index, fileData.preview, fileData.progress);

      // Remove button
      const removeBtn = card.querySelector('.rp-file-card__remove');
      removeBtn?.addEventListener('click', () => {
        removeFile(index);
      });

      filesList.appendChild(card);
    });

    // Dispatch change event
    container.dispatchEvent(new CustomEvent('rp-files-changed', {
      detail: { files: state.files.map(fd => fd.file) }
    }));
  }

  // Remove file
  function removeFile(index) {
    state.files.splice(index, 1);
    renderFiles();
  }

  // Public API
  container.rpFileUpload = {
    getFiles: () => state.files.map(fd => fd.file),
    clear: () => {
      state.files = [];
      renderFiles();
    },
    addFiles: (files) => handleFiles(Array.from(files))
  };
}

// Auto-enhance on load
export function enhanceFileUploads(root = document) {
  const containers = root.querySelectorAll('[data-rp-file-upload]');
  containers.forEach(enhanceFileUpload);
}

// Auto-initialize
if (typeof window !== 'undefined') {
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => enhanceFileUploads());
  } else {
    enhanceFileUploads();
  }
}
