<template>
  <div class="item-card" :class="`status-${item.status?.toLowerCase()}`">
    <!-- Status bar top -->
    <div class="status-stripe" />

    <div class="card-body">
      <!-- Image or placeholder -->
      <div class="item-img-wrap">
        <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.title" class="item-img" />
        <div v-else class="item-img-placeholder">
          <q-icon name="shopping_bag" size="28px" />
        </div>
      </div>

      <!-- Content -->
      <div class="item-content">
        <div class="item-top-row">
          <span class="item-domain">{{ item.domain || shortDomain(item.url) }}</span>
          <div class="item-actions">
            <q-btn
              flat
              round
              dense
              size="xs"
              icon="edit"
              @click="$emit('edit', item)"
              title="Edit"
            />
            <q-btn
              flat
              round
              dense
              size="xs"
              icon="delete_outline"
              @click="$emit('delete', item.id)"
              title="Delete"
            />
          </div>
        </div>

        <h3 class="item-title">{{ item.title }}</h3>

        <p v-if="item.notes" class="item-notes">{{ item.notes }}</p>

        <div class="item-bottom-row">
          <span v-if="item.price != null" class="item-price">
            {{ formatPrice(item.price, item.currency) }}
          </span>
          <a v-if="item.url" :href="item.url" target="_blank" rel="noopener" class="item-link">
            <q-icon name="open_in_new" size="14px" /> visit
          </a>
        </div>

        <!-- Tags -->
        <div v-if="item.tags && item.tags.length" class="item-tags">
          <span v-for="tag in item.tags" :key="tag" class="item-tag">{{ tag }}</span>
        </div>
      </div>
    </div>

    <!-- Footer: status selector -->
    <div class="card-footer">
      <div class="status-indicator">
        <span class="status-dot" />
        <span class="status-text">{{ item.status?.toLowerCase() }}</span>
      </div>
      <div class="status-actions">
        <q-btn
          v-for="s in otherStatuses"
          :key="s"
          flat
          dense
          no-caps
          size="xs"
          class="status-change-btn"
          :label="s.toLowerCase()"
          @click="$emit('status', item.id, s)"
        />
      </div>
    </div>
  </div>
</template>

<script>
const ALL_STATUSES = ['ACTIVE', 'ARCHIVED', 'BOUGHT']

export default {
  name: 'WishlistItemCard',
  props: {
    item: { type: Object, required: true },
  },
  emits: ['edit', 'delete', 'status'],
  computed: {
    otherStatuses() {
      return ALL_STATUSES.filter((s) => s !== this.item.status?.toUpperCase())
    },
  },
  methods: {
    shortDomain(url) {
      if (!url) return ''
      try {
        return new URL(url).hostname.replace('www.', '')
      } catch {
        return ''
      }
    },
    formatPrice(price, currency) {
      const curr = currency || 'USD'
      try {
        return new Intl.NumberFormat('en-US', { style: 'currency', currency: curr }).format(price)
      } catch {
        return `${curr} ${price}`
      }
    },
  },
}
</script>

<style scoped>
.item-card {
  background: var(--c-bg-alt);
  border: 1px solid var(--c-border);
  border-radius: 12px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  transition:
    transform 0.15s,
    box-shadow 0.15s;
  position: relative;
}
.item-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 28px rgba(0, 0, 0, 0.22);
}

/* Status stripe */
.status-stripe {
  height: 3px;
  width: 100%;
  background: var(--c-border);
}
.status-active .status-stripe {
  background: #6fcf97;
}
.status-bought .status-stripe {
  background: var(--c-accent);
}
.status-archived .status-stripe {
  background: #555;
}

.card-body {
  display: flex;
  gap: 0.9rem;
  padding: 1rem 1rem 0.6rem;
  flex: 1;
}

/* Image */
.item-img-wrap {
  flex-shrink: 0;
  width: 64px;
  height: 64px;
  border-radius: 8px;
  overflow: hidden;
  background: var(--c-bg);
  border: 1px solid var(--c-border);
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--c-text-muted);
}
.item-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.item-content {
  flex: 1;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.item-top-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.item-domain {
  font-size: 0.68rem;
  letter-spacing: 0.07em;
  text-transform: uppercase;
  color: var(--c-text-muted);
  opacity: 0.7;
}
.item-actions {
  display: flex;
  gap: 0.1rem;
  color: var(--c-text-muted);
  opacity: 0;
  transition: opacity 0.15s;
}
.item-card:hover .item-actions {
  opacity: 1;
}

.item-title {
  font-family: 'Playfair Display', serif;
  font-size: 0.95rem;
  font-weight: 600;
  margin: 0;
  color: var(--c-text);
  line-height: 1.3;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.item-notes {
  font-size: 0.78rem;
  color: var(--c-text-muted);
  margin: 0;
  line-height: 1.45;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.item-bottom-row {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}
.item-price {
  font-size: 0.88rem;
  font-weight: 700;
  color: var(--c-text);
  letter-spacing: -0.01em;
}
.item-link {
  font-size: 0.72rem;
  color: var(--c-accent);
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: 0.2rem;
  opacity: 0.8;
}
.item-link:hover {
  opacity: 1;
}

.item-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
  margin-top: 0.1rem;
}
.item-tag {
  font-size: 0.65rem;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  padding: 1px 7px;
  border-radius: 20px;
  background: var(--c-accent-soft);
  color: var(--c-accent);
}

/* Footer */
.card-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.5rem 1rem;
  border-top: 1px solid var(--c-border);
  margin-top: 0.4rem;
}
.status-indicator {
  display: flex;
  align-items: center;
  gap: 0.35rem;
}
.status-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--c-text-muted);
}
.status-active .status-dot {
  background: #6fcf97;
}
.status-bought .status-dot {
  background: var(--c-accent);
}
.status-archived .status-dot {
  background: #666;
}

.status-text {
  font-size: 0.68rem;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--c-text-muted);
}
.status-actions {
  display: flex;
  gap: 0.25rem;
}
.status-change-btn {
  font-size: 0.65rem !important;
  letter-spacing: 0.06em;
  color: var(--c-text-muted) !important;
  padding: 0 6px !important;
  border: 1px solid var(--c-border);
  border-radius: 4px;
}
.status-change-btn:hover {
  color: var(--c-accent) !important;
  border-color: var(--c-accent);
}
</style>
