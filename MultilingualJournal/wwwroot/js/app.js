let currentEntry = null;

// API Base URL
const API_BASE = '/api';

// Initialize the app
document.addEventListener('DOMContentLoaded', function() {
    showHome();
    loadEntries();
});

// Navigation functions
function showHome() {
    hideAllViews();
    document.getElementById('home-view').style.display = 'block';
    loadEntries();
}

function showNewEntry() {
    hideAllViews();
    document.getElementById('new-entry-view').style.display = 'block';
    document.getElementById('entry-form').reset();
}

function showTags() {
    hideAllViews();
    document.getElementById('tags-view').style.display = 'block';
    loadTags();
}

function hideAllViews() {
    document.getElementById('home-view').style.display = 'none';
    document.getElementById('new-entry-view').style.display = 'none';
    document.getElementById('tags-view').style.display = 'none';
}

// Load journal entries
async function loadEntries() {
    try {
        const response = await fetch(`${API_BASE}/journalentry`);
        const entries = await response.json();
        displayEntries(entries);
    } catch (error) {
        console.error('Error loading entries:', error);
        showError('Failed to load entries');
    }
}

// Display entries
function displayEntries(entries) {
    const container = document.getElementById('entries-container');
    
    if (entries.length === 0) {
        container.innerHTML = '<div class="alert alert-info">No entries found. Create your first entry!</div>';
        return;
    }

    container.innerHTML = entries.map(entry => `
        <div class="card entry-card position-relative" onclick="viewEntry(${entry.id})">
            <div class="language-indicator">${entry.language.toUpperCase()}</div>
            <div class="card-body">
                <h5 class="card-title">${escapeHtml(entry.title)}</h5>
                <p class="card-text">${escapeHtml(entry.content.substring(0, 150))}${entry.content.length > 150 ? '...' : ''}</p>
                <div class="entry-meta">
                    <small>Created: ${formatDate(entry.createdAt)}</small>
                    ${entry.tags && entry.tags.length > 0 ? `
                        <div class="mt-2">
                            ${entry.tags.map(tag => `
                                <span class="badge tag-badge ${tag.isMilestone ? 'tag-milestone' : 'tag-regular'}">${escapeHtml(tag.name)}</span>
                            `).join('')}
                        </div>
                    ` : ''}
                </div>
            </div>
        </div>
    `).join('');
}

// View entry details
async function viewEntry(id) {
    try {
        const response = await fetch(`${API_BASE}/journalentry/${id}`);
        const entry = await response.json();
        currentEntry = entry;
        showEntryModal(entry);
    } catch (error) {
        console.error('Error loading entry:', error);
        showError('Failed to load entry details');
    }
}

// Show entry in modal
function showEntryModal(entry) {
    document.getElementById('entryModalTitle').textContent = entry.title;
    
    let modalBody = `
        <div class="language-indicator">${entry.language.toUpperCase()}</div>
        <div class="mb-3">
            <h6>Content:</h6>
            <p>${escapeHtml(entry.content).replace(/\n/g, '<br>')}</p>
        </div>
        <div class="mb-3">
            <small class="text-muted">Created: ${formatDate(entry.createdAt)}</small>
            ${entry.updatedAt !== entry.createdAt ? `<br><small class="text-muted">Updated: ${formatDate(entry.updatedAt)}</small>` : ''}
        </div>
    `;

    if (entry.tags && entry.tags.length > 0) {
        modalBody += `
            <div class="mb-3">
                <h6>Tags:</h6>
                ${entry.tags.map(tag => `
                    <span class="badge tag-badge ${tag.isMilestone ? 'tag-milestone' : 'tag-regular'}">${escapeHtml(tag.name)}</span>
                `).join('')}
            </div>
        `;
    }

    if (entry.translations && entry.translations.length > 0) {
        modalBody += `
            <div class="translation-section">
                <h6>Translations:</h6>
                ${entry.translations.map(translation => `
                    <div class="translation-item">
                        <strong>${translation.targetLanguage.toUpperCase()}:</strong> ${escapeHtml(translation.translatedTitle)}<br>
                        <p class="mt-2">${escapeHtml(translation.translatedContent)}</p>
                        <small class="text-muted">Translated: ${formatDate(translation.createdAt)}</small>
                    </div>
                `).join('')}
            </div>
        `;
    }

    document.getElementById('entryModalBody').innerHTML = modalBody;
    
    const modal = new bootstrap.Modal(document.getElementById('entryModal'));
    modal.show();
}

// Translate current entry
async function translateEntry() {
    if (!currentEntry) return;

    const targetLanguage = prompt('Enter target language code (es, fr, de, it, pt):');
    if (!targetLanguage) return;

    try {
        const response = await fetch(`${API_BASE}/translation/translate-entry/${currentEntry.id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ targetLanguage })
        });

        if (response.ok) {
            showSuccess('Translation completed successfully!');
            viewEntry(currentEntry.id); // Refresh the modal with new translation
        } else {
            showError('Translation failed');
        }
    } catch (error) {
        console.error('Error translating entry:', error);
        showError('Translation failed');
    }
}

// Handle form submission
document.getElementById('entry-form').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const formData = {
        title: document.getElementById('title').value,
        content: document.getElementById('content').value,
        language: document.getElementById('language').value,
        tags: []
    };

    // Process tags
    const tagsInput = document.getElementById('tags').value;
    if (tagsInput.trim()) {
        const tagNames = tagsInput.split(',').map(t => t.trim()).filter(t => t);
        formData.tags = tagNames.map(tagName => ({
            name: tagName.startsWith('milestone:') ? tagName.substring(10) : tagName,
            isMilestone: tagName.startsWith('milestone:'),
            color: tagName.startsWith('milestone:') ? '#dc3545' : '#007bff'
        }));
    }

    try {
        const response = await fetch(`${API_BASE}/journalentry`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            showSuccess('Entry saved successfully!');
            showHome();
        } else {
            showError('Failed to save entry');
        }
    } catch (error) {
        console.error('Error saving entry:', error);
        showError('Failed to save entry');
    }
});

// Search entries
async function searchEntries() {
    const query = document.getElementById('search-input').value;
    
    try {
        const response = await fetch(`${API_BASE}/journalentry/search?query=${encodeURIComponent(query)}`);
        const entries = await response.json();
        displayEntries(entries);
    } catch (error) {
        console.error('Error searching entries:', error);
        showError('Search failed');
    }
}

// Load tags
async function loadTags() {
    try {
        const [tagsResponse, gitTagsResponse] = await Promise.all([
            fetch(`${API_BASE}/tag`),
            fetch(`${API_BASE}/tag/git-tags`)
        ]);
        
        const tags = await tagsResponse.json();
        const gitTags = await gitTagsResponse.json();
        
        displayTags(tags, gitTags);
    } catch (error) {
        console.error('Error loading tags:', error);
        showError('Failed to load tags');
    }
}

// Display tags
function displayTags(tags, gitTags = []) {
    const milestones = tags.filter(t => t.isMilestone);
    const regular = tags.filter(t => !t.isMilestone);

    document.getElementById('milestone-tags').innerHTML = milestones.length > 0 ? 
        milestones.map(tag => {
            const gitTagName = `milestone-${tag.name.toLowerCase().replace(/[^a-z0-9]/g, '-')}`;
            const hasGitTag = gitTags.includes(gitTagName);
            return `
                <div class="card mb-2">
                    <div class="card-body">
                        <span class="badge tag-milestone">${escapeHtml(tag.name)}</span>
                        <small class="text-muted">(${tag.journalEntries ? tag.journalEntries.length : 0} entries)</small>
                        ${hasGitTag ? '<span class="badge bg-success ms-2" title="Git tag created">📋 Git Tagged</span>' : ''}
                        <div class="mt-1">
                            <small class="text-muted">Created: ${formatDate(tag.createdAt)}</small>
                        </div>
                    </div>
                </div>
            `;
        }).join('') : '<p class="text-muted">No milestone tags yet.</p>';

    document.getElementById('regular-tags').innerHTML = regular.length > 0 ? 
        regular.map(tag => `
            <div class="card mb-2">
                <div class="card-body">
                    <span class="badge tag-regular">${escapeHtml(tag.name)}</span>
                    <small class="text-muted">(${tag.journalEntries ? tag.journalEntries.length : 0} entries)</small>
                    <div class="mt-1">
                        <small class="text-muted">Created: ${formatDate(tag.createdAt)}</small>
                    </div>
                </div>
            </div>
        `).join('') : '<p class="text-muted">No regular tags yet.</p>';

    // Display Git tags section
    if (gitTags.length > 0) {
        const gitTagsSection = `
            <div class="mt-4">
                <h5>Git Development Tags</h5>
                <div class="alert alert-info">
                    <small><strong>Info:</strong> These are Git tags automatically created for milestone achievements.</small>
                </div>
                <div class="d-flex flex-wrap gap-2">
                    ${gitTags.map(gitTag => `
                        <span class="badge bg-secondary">${escapeHtml(gitTag)}</span>
                    `).join('')}
                </div>
            </div>
        `;
        document.getElementById('milestone-tags').innerHTML += gitTagsSection;
    }
}

// Utility functions
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function showSuccess(message) {
    // You can implement a toast notification here
    alert(message);
}

function showError(message) {
    // You can implement a toast notification here
    alert('Error: ' + message);
}