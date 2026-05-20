<template>
  <div class="auth-page">
    <div class="auth-card">
      <div class="auth-brand">
        <div class="brand-star">✦</div>
        <h1 class="brand-name">wishlist</h1>
        <p class="brand-sub">sign in to your collection</p>
      </div>

      <q-form ref="formRef" @submit.prevent="handleSubmit">
        <div class="field-group">
          <label class="field-label">username</label>
          <q-input
            v-model="form.userName"
            outlined
            dense
            :rules="[(v) => !!v || 'Required']"
            autocomplete="username"
          />
        </div>

        <div class="field-group">
          <label class="field-label">password</label>
          <q-input
            v-model="form.password"
            outlined
            dense
            type="password"
            :rules="[(v) => !!v || 'Required']"
            autocomplete="current-password"
          />
        </div>

        <div v-if="errorMsg" class="error-banner">{{ errorMsg }}</div>

        <q-btn
          unelevated
          no-caps
          type="submit"
          label="sign in"
          :loading="loading"
          class="auth-btn"
        />
      </q-form>

      <p class="auth-switch">
        No account? <router-link to="/register" class="auth-link">register here</router-link>
      </p>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '../stores/auth'

export default {
  name: 'LoginPage',
  setup() {
    return { authStore: useAuthStore() }
  },
  data() {
    return {
      form: { userName: '', password: '' },
      loading: false,
      errorMsg: '',
    }
  },
  methods: {
    async handleSubmit() {
      const valid = await this.$refs.formRef.validate()
      if (!valid) return
      this.errorMsg = ''
      this.loading = true
      try {
        await this.authStore.login(this.form)
        this.$router.push('/wishlist')
      } catch (e) {
        this.errorMsg = typeof e?.data === 'string' ? e.data : 'Invalid credentials'
      } finally {
        this.loading = false
      }
    },
  },
}
</script>

<style scoped>
.auth-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--c-bg);
  background-image: radial-gradient(ellipse at 20% 60%, var(--c-accent-soft) 0%, transparent 55%);
}
.auth-card {
  width: 360px;
  max-width: 94vw;
  padding: 2.5rem;
  background: var(--c-bg-alt);
  border: 1px solid var(--c-border);
  border-radius: 16px;
}
.auth-brand {
  text-align: center;
  margin-bottom: 2rem;
}
.brand-star {
  font-size: 1.8rem;
  color: var(--c-accent);
  line-height: 1;
}
.brand-name {
  font-family: 'Playfair Display', serif;
  font-size: 2rem;
  color: var(--c-accent);
  margin: 0.25rem 0 0;
  letter-spacing: 0.05em;
}
.brand-sub {
  font-size: 0.78rem;
  color: var(--c-text-muted);
  margin: 0.3rem 0 0;
  letter-spacing: 0.07em;
}
.field-group {
  margin-bottom: 0.9rem;
}
.field-label {
  display: block;
  font-size: 0.7rem;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: var(--c-text-muted);
  margin-bottom: 0.35rem;
}
.error-banner {
  background: rgba(255, 68, 68, 0.12);
  color: #ff7070;
  border-radius: 8px;
  padding: 0.6rem 0.9rem;
  font-size: 0.82rem;
  margin-bottom: 0.9rem;
}
.auth-btn {
  width: 100%;
  background: var(--c-accent);
  color: var(--c-bg);
  height: 44px;
  border-radius: 8px;
  font-size: 0.85rem;
  letter-spacing: 0.06em;
  margin-top: 0.3rem;
}
.auth-switch {
  text-align: center;
  margin-top: 1.2rem;
  font-size: 0.82rem;
  color: var(--c-text-muted);
}
.auth-link {
  color: var(--c-accent);
  text-decoration: none;
}
</style>
