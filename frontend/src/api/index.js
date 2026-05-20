const BASE = import.meta.env.VITE_API_URL ?? 'http://localhost:5110'

function getToken() {
  return localStorage.getItem('token')
}

async function request(method, path, body, params) {
  const url = new URL(`${BASE}${path}`)
  if (params) {
    Object.entries(params).forEach(([k, v]) => {
      if (v !== undefined && v !== null && v !== '') url.searchParams.set(k, v)
    })
  }
  const headers = { 'Content-Type': 'application/json' }
  const token = getToken()
  if (token) headers['Authorization'] = `Bearer ${token}`
  const res = await fetch(url, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  })
  if (res.status === 204) return null
  const data = await res.json().catch(() => null)
  if (!res.ok) throw { status: res.status, data }
  return data
}

export const auth = {
  login: (dto) => request('POST', '/api/account/login', dto),
  register: (dto) => request('POST', '/api/account/register', dto),
}

export const wishlist = {
  getAll: (query) => request('GET', '/api/wishlist-items', null, query),
  getById: (id) => request('GET', `/api/wishlist-items/${id}`),
  create: (dto) => request('POST', '/api/wishlist-items', dto),
  update: (id, dto) => request('PATCH', `/api/wishlist-items/${id}`, dto),
  updateStatus: (id, status) => request('PATCH', `/api/wishlist-items/${id}/status`, { status }),
  delete: (id) => request('DELETE', `/api/wishlist-items/${id}`),
}

export const tags = {
  getAll: () => request('GET', '/api/tags'),
  create: (dto) => request('POST', '/api/tags', dto),
  delete: (id) => request('DELETE', `/api/tags/${id}`),
}
