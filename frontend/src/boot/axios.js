import { boot } from 'quasar/wrappers'
import axios from 'axios'

const api = axios.create({ baseURL: 'http://localhost:5110' })

export default boot(({ app }) => {
  // Rehydrate token from localStorage on every page load/refresh
  const token = localStorage.getItem('auth_token')
  if (token) {
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`
  }

  app.config.globalProperties.$axios = axios
  app.config.globalProperties.$api = api
})

export { api }
