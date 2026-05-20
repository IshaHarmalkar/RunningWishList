<template>
  <q-page class="wish-page">
    <div class="page-inner">
      <!-- Header -->
      <div class="page-header">
        <div>
          <h1 class="page-title">my wishlist</h1>
          <p class="page-subtitle">
            {{ store.items.length }} item{{ store.items.length !== 1 ? 's' : '' }} shown
          </p>
        </div>
        <q-btn
          unelevated
          no-caps
          icon="add"
          label="add item"
          class="add-btn"
          @click="showForm = true"
        />
      </div>

      <!-- Filters -->
      <WishlistFilterBar :tags="store.tags" @filter="onFilter" class="q-mb-lg" />

      <!-- Loading -->
      <div v-if="store.loading" class="state-center">
        <q-spinner-dots color="var(--c-accent)" size="40px" />
      </div>

      <!-- Error -->
      <div v-else-if="store.error" class="state-center">
        <q-icon name="error_outline" size="32px" color="negative" />
        <p class="state-msg">Failed to load items</p>
        <q-btn flat no-caps label="retry" @click="load" />
      </div>

      <!-- Empty -->
      <div v-else-if="!store.items.length" class="state-center">
        <q-icon
          name="favorite_border"
          size="48px"
          style="color: var(--c-text-muted); opacity: 0.4"
        />
        <p class="state-msg">No items yet — add your first wish!</p>
      </div>

      <!-- Grid -->
      <div v-else class="items-grid">
        <WishlistItemCard
          v-for="item in store.items"
          :key="item.id"
          :item="item"
          @edit="openEdit"
          @delete="confirmDelete"
          @status="handleStatus"
        />
      </div>

      <!-- Pagination -->
      <div v-if="store.items.length" class="pagination-row">
        <q-btn
          flat
          dense
          no-caps
          icon="chevron_left"
          label="prev"
          :disable="currentPage <= 1"
          @click="changePage(currentPage - 1)"
        />
        <span class="page-indicator">page {{ currentPage }}</span>
        <q-btn
          flat
          dense
          no-caps
          icon-right="chevron_right"
          label="next"
          :disable="!store.pagination.hasMore"
          @click="changePage(currentPage + 1)"
        />
      </div>
    </div>

    <!-- Add/Edit dialog -->
    <ItemFormDialog
      v-model="showForm"
      :edit-item="editingItem"
      :available-tags="store.tags"
      @created="handleCreate"
      @updated="handleUpdate"
    />

    <!-- Delete confirm -->
    <q-dialog v-model="deleteDialog">
      <q-card class="confirm-card">
        <q-card-section>
          <p class="confirm-msg">Remove this item from your wishlist?</p>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat no-caps label="cancel" v-close-popup />
          <q-btn
            flat
            no-caps
            label="remove"
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
import WishlistItemCard from '../components/WishlistItemCard.vue'
import WishlistFilterBar from '../components/WishlistFilterBar.vue'
import ItemFormDialog from '../components/ItemFormDialog.vue'

export default {
  name: 'WishlistPage',
  components: { WishlistItemCard, WishlistFilterBar, ItemFormDialog },
  setup() {
    return { store: useWishlistStore() }
  },
  data() {
    return {
      showForm: false,
      editingItem: null,
      deleteDialog: false,
      deletingId: null,
      deleting: false,
      currentPage: 1,
      activeFilter: {},
    }
  },
  async created() {
    await this.load()
  },
  methods: {
    async load() {
      await Promise.all([
        this.store.fetchItems({ page: this.currentPage, ...this.activeFilter }),
        this.store.fetchTags(),
      ])
    },
    onFilter(params) {
      this.activeFilter = params
      this.currentPage = 1
      this.store.fetchItems({ page: 1, ...params })
    },
    changePage(p) {
      this.currentPage = p
      this.store.fetchItems({ page: p, ...this.activeFilter })
      window.scrollTo({ top: 0, behavior: 'smooth' })
    },
    openEdit(item) {
      this.editingItem = item
      this.showForm = true
    },
    confirmDelete(id) {
      this.deletingId = id
      this.deleteDialog = true
    },
    async doDelete() {
      this.deleting = true
      try {
        await this.store.deleteItem(this.deletingId)
        this.deleteDialog = false
        this.$q.notify({ message: 'Item removed', color: 'dark', position: 'bottom' })
      } finally {
        this.deleting = false
      }
    },
    async handleStatus(id, status) {
      await this.store.updateItemStatus(id, status)
      this.$q.notify({
        message: `Marked as ${status.toLowerCase()}`,
        color: 'dark',
        position: 'bottom',
      })
    },
    async handleCreate(dto) {
      try {
        await this.store.createItem(dto)
        this.$q.notify({ message: 'Item added!', color: 'positive', position: 'bottom' })
      } catch (e) {
        console.error('Delete tag error:', e)
        this.$q.notify({ message: 'Failed to add item', color: 'negative', position: 'bottom' })
      }
    },
    async handleUpdate({ id, dto }) {
      try {
        await this.store.updateItem(id, dto)
        this.editingItem = null
        this.$q.notify({ message: 'Item updated', color: 'dark', position: 'bottom' })
      } catch (e) {
        console.error('Delete tag error:', e)
        this.$q.notify({ message: 'Failed to update item', color: 'negative', position: 'bottom' })
      }
    },
  },
  watch: {
    showForm(val) {
      if (!val) this.editingItem = null
    },
  },
}
</script>

<style scoped>
.wish-page {
  background: var(--c-bg);
  min-height: 100vh;
}
.page-inner {
  max-width: 1100px;
  margin: 0 auto;
  padding: 2rem 1.5rem;
}
.page-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: 1.5rem;
  gap: 1rem;
}
.page-title {
  font-family: 'Playfair Display', serif;
  font-size: 2rem;
  margin: 0;
  color: var(--c-text);
  letter-spacing: -0.01em;
}
.page-subtitle {
  font-size: 0.78rem;
  color: var(--c-text-muted);
  margin: 0.2rem 0 0;
  letter-spacing: 0.04em;
}
.add-btn {
  background: var(--c-accent);
  color: var(--c-bg);
  height: 40px;
  padding: 0 1.2rem;
  border-radius: 8px;
  font-size: 0.82rem;
  flex-shrink: 0;
}
.items-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1rem;
}
.state-center {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 0;
  gap: 0.8rem;
}
.state-msg {
  font-size: 0.9rem;
  color: var(--c-text-muted);
  margin: 0;
}
.pagination-row {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  margin-top: 2rem;
  color: var(--c-text-muted);
}
.page-indicator {
  font-size: 0.82rem;
  letter-spacing: 0.06em;
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
</style>
