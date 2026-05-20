import { defineStore } from 'pinia'
import { auth as authApi } from '../api'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    user: JSON.parse(localStorage.getItem('user') || 'null'),
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    _setSession(data) {
      this.token = data.token
      this.user = { userName: data.userName, email: data.email }
      localStorage.setItem('token', data.token)
      localStorage.setItem('user', JSON.stringify(this.user))
    },
    async login(dto) {
      const data = await authApi.login(dto)
      this._setSession(data)
    },
    async register(dto) {
      const data = await authApi.register(dto)
      this._setSession(data)
    },
    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    },
  },
})
