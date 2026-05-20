<template>
  <q-layout view="lHh Lpr lFf">
    <q-header class="app-header">
      <q-toolbar>
        <q-btn flat dense round icon="menu" @click="drawer = !drawer" class="menu-btn" />
        <q-toolbar-title class="app-title">
          <span class="title-star">✦</span> wishlist
        </q-toolbar-title>
        <span class="username-badge">{{ authStore.user?.userName }}</span>
        <q-btn
          flat
          dense
          round
          icon="logout"
          @click="handleLogout"
          class="logout-btn"
          title="Logout"
        />
      </q-toolbar>
    </q-header>

    <q-drawer v-model="drawer" show-if-above :width="210" class="app-drawer">
      <div class="drawer-inner">
        <q-list padding>
          <q-item
            v-for="link in navLinks"
            :key="link.to"
            :to="link.to"
            clickable
            v-ripple
            active-class="nav-active"
            class="nav-item"
          >
            <q-item-section avatar>
              <q-icon :name="link.icon" size="18px" />
            </q-item-section>
            <q-item-section class="nav-label">{{ link.label }}</q-item-section>
          </q-item>
        </q-list>
      </div>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { useAuthStore } from '../stores/auth'

export default {
  name: 'MainLayout',
  setup() {
    return { authStore: useAuthStore() }
  },
  data() {
    return {
      drawer: false,
      navLinks: [
        { to: '/wishlist', icon: 'favorite_border', label: 'Wishlist' },
        { to: '/tags', icon: 'label_outline', label: 'Tags' },
      ],
    }
  },
  methods: {
    handleLogout() {
      this.authStore.logout()
      this.$router.push('/login')
    },
  },
}
</script>

<style scoped>
.app-header {
  background: var(--c-bg-alt);
  color: var(--c-text);
  border-bottom: 1px solid var(--c-border);
  box-shadow: none;
}
.app-title {
  font-family: 'Playfair Display', serif;
  font-size: 1.25rem;
  letter-spacing: 0.06em;
  color: var(--c-accent);
}
.title-star {
  font-size: 0.9rem;
  opacity: 0.7;
}
.menu-btn,
.logout-btn {
  color: var(--c-text-muted);
}
.username-badge {
  font-size: 0.75rem;
  letter-spacing: 0.06em;
  color: var(--c-text-muted);
  margin-right: 0.6rem;
  opacity: 0.8;
}
.app-drawer {
  background: var(--c-bg-alt);
  border-right: 1px solid var(--c-border);
}
.drawer-inner {
  padding-top: 0.5rem;
}
.nav-item {
  border-radius: 8px;
  margin: 2px 8px;
  min-height: 40px;
  color: var(--c-text-muted);
  font-size: 0.85rem;
}
.nav-label {
  font-size: 0.85rem;
  letter-spacing: 0.04em;
}
.nav-active {
  color: var(--c-accent) !important;
  background: var(--c-accent-soft) !important;
}
</style>
