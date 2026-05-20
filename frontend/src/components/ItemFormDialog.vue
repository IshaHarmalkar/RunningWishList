<template>
  <q-dialog v-model="isOpen" persistent>
    <q-card class="item-dialog">
      <q-card-section class="dialog-header">
        <span class="dialog-title">{{ isEdit ? 'edit item' : 'add to wishlist' }}</span>
        <q-btn flat round dense icon="close" size="sm" @click="close" />
      </q-card-section>

      <q-separator color="var(--c-border)" />

      <q-card-section class="dialog-body">
        <q-form ref="formRef" @submit.prevent="handleSubmit">
          <!-- URL (create only) -->
          <div v-if="!isEdit" class="field-group">
            <label class="field-label">url <span class="required">*</span></label>
            <q-input
              v-model="form.url"
              outlined
              dense
              placeholder="https://..."
              :rules="[(v) => !!v || 'Required', (v) => isValidUrl(v) || 'Must be a valid URL']"
            />
          </div>

          <!-- Title -->
          <div class="field-group">
            <label class="field-label">title <span class="required">*</span></label>
            <q-input
              v-model="form.title"
              outlined
              dense
              placeholder="Item name"
              :rules="[(v) => !!v || 'Required', (v) => v.length <= 300 || 'Max 300 chars']"
            />
          </div>

          <!-- Price + Currency row -->
          <div class="field-row">
            <div class="field-group" style="flex: 1">
              <label class="field-label">price</label>
              <q-input
                v-model.number="form.price"
                outlined
                dense
                type="number"
                min="0"
                step="0.01"
                placeholder="0.00"
              />
            </div>
            <div class="field-group" style="width: 110px">
              <label class="field-label">currency</label>
              <q-select
                v-model="form.currency"
                outlined
                dense
                :options="currencies"
                emit-value
                map-options
              />
            </div>
          </div>

          <!-- Image URL -->
          <div class="field-group">
            <label class="field-label">image url</label>
            <q-input
              v-model="form.imageUrl"
              outlined
              dense
              placeholder="https://..."
              :rules="[(v) => !v || isValidUrl(v) || 'Must be a valid URL']"
            />
          </div>

          <!-- Notes -->
          <div class="field-group">
            <label class="field-label">notes</label>
            <q-input
              v-model="form.notes"
              outlined
              dense
              type="textarea"
              rows="2"
              placeholder="Any notes..."
            />
          </div>

          <!-- Tags -->
          <div class="field-group">
            <label class="field-label">tags</label>
            <div class="tags-input-wrap">
              <div class="selected-tags">
                <span v-for="tag in form.tags" :key="tag" class="selected-tag">
                  {{ tag }}
                  <q-icon name="close" size="10px" class="tag-remove" @click="removeTag(tag)" />
                </span>
              </div>
              <div class="tag-add-row">
                <q-input
                  v-model="tagInput"
                  outlined
                  dense
                  placeholder="Add tag..."
                  @keydown.enter.prevent="addTag"
                  style="flex: 1"
                />
                <q-btn flat dense no-caps icon="add" @click="addTag" class="add-tag-btn" />
              </div>
              <!-- existing tags quick-add -->
              <div v-if="availableTags.length" class="available-tags">
                <span
                  v-for="t in availableTags"
                  :key="t.id"
                  class="avail-tag"
                  :class="{ active: form.tags.includes(t.name) }"
                  @click="toggleTag(t.name)"
                  >{{ t.name }}</span
                >
              </div>
            </div>
          </div>

          <div class="dialog-footer">
            <q-btn flat no-caps label="cancel" @click="close" class="cancel-btn" />
            <q-btn
              unelevated
              no-caps
              :label="isEdit ? 'save changes' : 'add item'"
              :loading="loading"
              type="submit"
              class="submit-btn"
            />
          </div>
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
const EMPTY_FORM = () => ({
  url: '',
  title: '',
  price: null,
  currency: 'USD',
  imageUrl: '',
  notes: '',
  tags: [],
})

export default {
  name: 'ItemFormDialog',
  props: {
    modelValue: { type: Boolean, default: false },
    editItem: { type: Object, default: null },
    availableTags: { type: Array, default: () => [] },
  },
  emits: ['update:modelValue', 'created', 'updated'],
  data() {
    return {
      form: EMPTY_FORM(),
      tagInput: '',
      loading: false,
      currencies: ['USD', 'EUR', 'GBP', 'INR', 'JPY', 'CAD', 'AUD'],
    }
  },
  computed: {
    isOpen: {
      get() {
        return this.modelValue
      },
      set(v) {
        this.$emit('update:modelValue', v)
      },
    },
    isEdit() {
      return !!this.editItem
    },
  },
  watch: {
    modelValue(val) {
      if (val) this.resetForm()
    },
  },
  methods: {
    resetForm() {
      if (this.editItem) {
        this.form = {
          url: this.editItem.url || '',
          title: this.editItem.title || '',
          price: this.editItem.price ?? null,
          currency: this.editItem.currency || 'USD',
          imageUrl: this.editItem.imageUrl || '',
          notes: this.editItem.notes || '',
          tags: [...(this.editItem.tags || [])],
        }
      } else {
        this.form = EMPTY_FORM()
      }
      this.tagInput = ''
    },
    close() {
      this.isOpen = false
    },
    isValidUrl(v) {
      if (!v) return true
      try {
        new URL(v)
        return true
      } catch {
        return false
      }
    },
    addTag() {
      const t = this.tagInput.trim()
      if (t && !this.form.tags.includes(t)) this.form.tags.push(t)
      this.tagInput = ''
    },
    removeTag(tag) {
      this.form.tags = this.form.tags.filter((t) => t !== tag)
    },
    toggleTag(name) {
      if (this.form.tags.includes(name)) {
        this.removeTag(name)
      } else {
        this.form.tags.push(name)
      }
    },
    async handleSubmit() {
      const valid = await this.$refs.formRef.validate()
      if (!valid) return
      this.loading = true
      try {
        if (this.isEdit) {
          const dto = {
            title: this.form.title || undefined,
            price: this.form.price ?? undefined,
            currency: this.form.currency || undefined,
            imageUrl: this.form.imageUrl || undefined,
            notes: this.form.notes || undefined,
            tags: this.form.tags,
          }
          this.$emit('updated', { id: this.editItem.id, dto })
        } else {
          const dto = {
            url: this.form.url,
            title: this.form.title,
            price: this.form.price ?? undefined,
            currency: this.form.currency || undefined,
            imageUrl: this.form.imageUrl || undefined,
            notes: this.form.notes || undefined,
            tags: this.form.tags,
          }
          this.$emit('created', dto)
        }
        this.close()
      } finally {
        this.loading = false
      }
    },
  },
}
</script>

<style scoped>
.item-dialog {
  width: 480px;
  max-width: 96vw;
  /* background: var(--c-bg-alt); */
  background: whitesmoke;
  opacity: 0.9;

  color: var(--c-text);
  border: 1px solid var(--c-border);
  border-radius: 14px;
}
.dialog-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 1.2rem 0.8rem;
}
.dialog-title {
  font-family: 'Playfair Display', serif;
  font-size: 1.1rem;
  color: var(--c-text);
}
.dialog-body {
  padding: 1rem 1.2rem;
}

.field-group {
  margin-bottom: 0.9rem;
}
.field-row {
  display: flex;
  gap: 0.8rem;
}

.field-label {
  display: block;
  font-size: 0.7rem;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: var(--c-text-muted);
  margin-bottom: 0.35rem;
}
.required {
  color: var(--c-accent);
}

.tags-input-wrap {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.selected-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.3rem;
  min-height: 24px;
}
.selected-tag {
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  font-size: 0.7rem;
  letter-spacing: 0.06em;
  padding: 2px 8px;
  border-radius: 20px;
  background: var(--c-accent-soft);
  color: var(--c-accent);
  text-transform: uppercase;
}
.tag-remove {
  cursor: pointer;
  opacity: 0.7;
}
.tag-remove:hover {
  opacity: 1;
}

.tag-add-row {
  display: flex;
  gap: 0.4rem;
  align-items: flex-start;
}
.add-tag-btn {
  color: var(--c-accent) !important;
  margin-top: 2px;
}

.available-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
}
.avail-tag {
  font-size: 0.68rem;
  padding: 2px 8px;
  border-radius: 20px;
  border: 1px solid var(--c-border);
  color: var(--c-text-muted);
  cursor: pointer;
  transition: all 0.12s;
  letter-spacing: 0.05em;
}
.avail-tag:hover,
.avail-tag.active {
  border-color: var(--c-accent);
  color: var(--c-accent);
  background: var(--c-accent-soft);
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.6rem;
  margin-top: 1.2rem;
}
.cancel-btn {
  color: var(--c-text-muted) !important;
  font-size: 0.82rem;
}
.submit-btn {
  background: var(--c-accent);
  color: var(--c-bg);
  padding: 0 1.2rem;
  height: 38px;
  border-radius: 8px;
  font-size: 0.82rem;
}
</style>
