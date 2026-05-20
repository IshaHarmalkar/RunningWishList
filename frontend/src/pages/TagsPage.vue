<template>
  <q-page class="tags-page">
    <div class="page-inner">
      <div class="page-header">
        <div>
          <h1 class="page-title">tags</h1>
          <p class="page-subtitle">organise your wishlist</p>
        </div>
      </div>

      <!-- Add tag form -->
      <div class="add-tag-card">
        <q-form @submit.prevent="handleCreate" class="add-tag-form">
          <div class="field-group">
            <label class="field-label">new tag</label>
            <div class="add-tag-row">
              <q-input
                v-model="newTagName"
                outlined
                dense
                placeholder="e.g. books, tech, clothing..."
                :rules="[(v) => !!v || 'Required', (v) => v.length <= 50 || 'Max 50 chars']"
                style="flex: 1"
              />
              <q-btn
                unelevated
                no-caps
                icon="add"
                label="create"
                type="submit"
                :loading="creating"
                class="create-btn"
              />
            </div>
          </div>
        </q-form>
      </div>

      <!-- Loading -->
      <div v-if="loading" class="state-center">
        <q-spinner-dots color="var(--c-accent)" size="36px" />
      </div>

      <!-- Empty -->
      <div v-else-if="!store.tags.length" class="state-center">
        <q-icon name="label_outline" size="42px" style="color: var(--c-text-muted); opacity: 0.4" />
        <p class="state-msg">No tags yet</p>
      </div>

      <!-- Tags list -->
      <div v-else class="tags-list">
        <div v-for="tag in store.tags" :key="tag.id" class="tag-row">
          <div class="tag-info">
            <span class="tag-chip">{{ tag.name }}</span>
            <span class="tag-count">{{ tag.count }} item{{ tag.count !== 1 ? 's' : '' }}</span>
          </div>
          <q-btn
            flat
            round
            dense
            size="sm"
            icon="delete_outline"
            class="delete-tag-btn"
            @click="confirmDelete(tag)"
            title="Delete tag"
          />
        </div>
      </div>
    </div>

    <!-- Delete confirm -->
    <q-dialog v-model="deleteDialog">
      <q-card class="confirm-card">
        <q-card-section>
          <p class="confirm-msg">
            Delete tag <strong>{{ deletingTag?.name }}</strong
            >?
            <span v-if="deletingTag?.count" class="confirm-sub">
              It's used on {{ deletingTag.count }} item{{ deletingTag.count !== 1 ? 's' : '' }}.
            </span>
          </p>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat no-caps label="cancel" v-close-popup />
          <q-btn
            flat
            no-caps
            label="delete"
            color="negative"
            :loading="deleting"
            @click="doDelete"
          />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { useWishlistStore } from '../stores/wishlist'

export default {
  name: 'TagsPage',
  setup() {
    return { store: useWishlistStore() }
  },
  data() {
    return {
      newTagName: '',
      creating: false,
      loading: false,
      deleteDialog: false,
      deletingTag: null,
      deleting: false,
    }
  },
  async created() {
    this.loading = true
    await this.store.fetchTags()
    this.loading = false
  },
  methods: {
    async handleCreate() {
      const name = this.newTagName.trim()
      if (!name) return
      this.creating = true
      try {
        await this.store.createTag(name)
        this.newTagName = ''
        this.$q.notify({ message: `Tag "${name}" created`, color: 'positive', position: 'bottom' })
      } catch (e) {
        const msg = e?.data?.message || 'Failed to create tag'
        this.$q.notify({ message: msg, color: 'negative', position: 'bottom' })
      } finally {
        this.creating = false
      }
    },
    confirmDelete(tag) {
      this.deletingTag = tag
      this.deleteDialog = true
    },
    async doDelete() {
      this.deleting = true
      try {
        await this.store.deleteTag(this.deletingTag.id)
        this.deleteDialog = false
        this.$q.notify({ message: 'Tag deleted', color: 'dark', position: 'bottom' })
      } catch (e) {
        console.error('Delete tag error:', e)
        this.$q.notify({ message: 'Failed to delete tag', color: 'negative', position: 'bottom' })
      } finally {
        this.deleting = false
      }
    },
  },
}
</script>

<style scoped>
.tags-page {
  background: var(--c-bg);
  min-height: 100vh;
}
.page-inner {
  max-width: 640px;
  margin: 0 auto;
  padding: 2rem 1.5rem;
}

.page-header {
  margin-bottom: 1.5rem;
}
.page-title {
  font-family: 'Playfair Display', serif;
  font-size: 2rem;
  margin: 0;
  color: var(--c-text);
}
.page-subtitle {
  font-size: 0.78rem;
  color: var(--c-text-muted);
  margin: 0.2rem 0 0;
}

.add-tag-card {
  background: var(--c-bg-alt);
  border: 1px solid var(--c-border);
  border-radius: 12px;
  padding: 1.2rem;
  margin-bottom: 1.5rem;
}
.add-tag-row {
  display: flex;
  gap: 0.6rem;
  align-items: flex-start;
}
.field-group {
  margin: 0;
}
.field-label {
  display: block;
  font-size: 0.7rem;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: var(--c-text-muted);
  margin-bottom: 0.4rem;
}
.create-btn {
  background: var(--c-accent);
  color: var(--c-bg);
  height: 40px;
  padding: 0 1rem;
  border-radius: 8px;
  font-size: 0.82rem;
  flex-shrink: 0;
}

.tags-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.tag-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.75rem 1rem;
  background: var(--c-bg-alt);
  border: 1px solid var(--c-border);
  border-radius: 10px;
  transition: border-color 0.15s;
}
.tag-row:hover {
  border-color: var(--c-accent);
}
.tag-info {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}

.tag-chip {
  font-size: 0.78rem;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  padding: 3px 10px;
  border-radius: 20px;
  background: var(--c-accent-soft);
  color: var(--c-accent);
}
.tag-count {
  font-size: 0.78rem;
  color: var(--c-text-muted);
}
.delete-tag-btn {
  color: var(--c-text-muted) !important;
  opacity: 0.5;
}
.delete-tag-btn:hover {
  opacity: 1;
  color: #ff6b6b !important;
}

.state-center {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem 0;
  gap: 0.8rem;
}
.state-msg {
  font-size: 0.9rem;
  color: var(--c-text-muted);
  margin: 0;
}

.confirm-card {
  background: var(--c-bg-alt);
  border: 1px solid var(--c-border);
  border-radius: 12px;
  min-width: 280px;
  color: var(--c-text);
}
.confirm-msg {
  font-size: 0.9rem;
  margin: 0;
}
.confirm-sub {
  display: block;
  font-size: 0.8rem;
  color: var(--c-text-muted);
  margin-top: 0.3rem;
}
</style>
