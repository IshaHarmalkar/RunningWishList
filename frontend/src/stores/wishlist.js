import { defineStore } from 'pinia'
import { wishlist as api, tags as tagsApi } from '../api'

export const useWishlistStore = defineStore('wishlist', {
  state: () => ({
    items: [],
    pagination: { page: 1, hasMore: false },
    tags: [],
    loading: false,
    error: null,
    query: {
      status: '',
      tag: '',
      search: '',
      page: 1,
      pageSize: 10,
    },
  }),
  actions: {
    async fetchItems(queryOverride = {}) {
      this.loading = true
      this.error = null
      Object.assign(this.query, queryOverride)
      try {
        const res = await api.getAll(this.query)
        this.items = res.items
        this.pagination = res.pagination
      } catch (e) {
        this.error = e
      } finally {
        this.loading = false
      }
    },
    async fetchTags() {
      this.tags = await tagsApi.getAll()
    },
    async createItem(dto) {
      await api.create(dto)
      await this.fetchItems({ page: 1 })
    },
    async updateItem(id, dto) {
      const updated = await api.update(id, dto)
      const idx = this.items.findIndex((i) => i.id === id)
      if (idx !== -1) this.items[idx] = updated
      return updated
    },
    async updateItemStatus(id, status) {
      await api.updateStatus(id, status)
      const idx = this.items.findIndex((i) => i.id === id)
      if (idx !== -1) this.items[idx].status = status.toUpperCase()
    },
    async deleteItem(id) {
      await api.delete(id)
      this.items = this.items.filter((i) => i.id !== id)
    },
    async createTag(name) {
      await tagsApi.create({ name })
      await this.fetchTags()
    },
    async deleteTag(id) {
      await tagsApi.delete(id)
      this.tags = this.tags.filter((t) => t.id !== id)
    },
  },
})
