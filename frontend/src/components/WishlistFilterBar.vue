<template>
  <div class="filter-bar">
    <q-input
      v-model="local.search"
      outlined
      dense
      clearable
      placeholder="search items..."
      class="filter-search"
      debounce="350"
      @update:model-value="emit"
    >
      <template #prepend>
        <q-icon name="search" size="18px" />
      </template>
    </q-input>

    <q-select
      v-model="local.status"
      outlined
      dense
      clearable
      :options="statusOptions"
      placeholder="status"
      class="filter-select"
      emit-value
      map-options
      @update:model-value="emit"
    />

    <q-select
      v-model="local.tag"
      outlined
      dense
      clearable
      :options="tagOptions"
      option-label="label"
      option-value="value"
      placeholder="tag"
      class="filter-select"
      emit-value
      map-options
      @update:model-value="emit"
    />

    <q-select
      v-model="local.pageSize"
      outlined
      dense
      :options="pageSizeOptions"
      class="filter-pagesize"
      emit-value
      map-options
      @update:model-value="emit"
    />
  </div>
</template>

<script>
export default {
  name: 'WishlistFilterBar',
  props: {
    tags: { type: Array, default: () => [] },
  },
  emits: ['filter'],
  data() {
    return {
      local: {
        search: '',
        status: null,
        tag: null,
        pageSize: 10,
      },
      statusOptions: [
        { label: 'Active', value: 'ACTIVE' },
        { label: 'Bought', value: 'BOUGHT' },
        { label: 'Archived', value: 'ARCHIVED' },
      ],
      pageSizeOptions: [
        { label: '10 / page', value: 10 },
        { label: '20 / page', value: 20 },
      ],
    }
  },
  computed: {
    tagOptions() {
      return this.tags.map((t) => ({ label: t.name, value: t.name }))
    },
  },
  methods: {
    emit() {
      this.$emit('filter', {
        search: this.local.search || undefined,
        status: this.local.status || undefined,
        tag: this.local.tag || undefined,
        pageSize: this.local.pageSize,
        page: 1,
      })
    },
  },
}
</script>

<style scoped>
.filter-bar {
  display: flex;
  align-items: flex-start;
  gap: 0.6rem;
  flex-wrap: wrap;
}
.filter-search {
  flex: 1;
  min-width: 180px;
}
.filter-select {
  width: 140px;
}
.filter-pagesize {
  width: 120px;
}
</style>
