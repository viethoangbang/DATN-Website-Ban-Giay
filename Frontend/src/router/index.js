import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('../views/auth/Login.vue'),
    meta: { requiresGuest: true }
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../views/auth/Register.vue'),
    meta: { requiresGuest: true }
  },
  {
    path: '/',
    name: 'Home',
    component: () => import('../views/user/Home.vue'),
  },
  {
    path: '/products',
    name: 'Products',
    component: () => import('../views/user/Products.vue'),
  },
  {
    path: '/products/:id',
    name: 'ProductDetail',
    component: () => import('../views/user/ProductDetail.vue'),
  },
  {
    path: '/cart',
    name: 'Cart',
    component: () => import('../views/user/Cart.vue'),
  },
  {
    path: '/checkout',
    name: 'Checkout',
    component: () => import('../views/user/Checkout.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/orders',
    name: 'Orders',
    component: () => import('../views/user/Orders.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/orders/:id',
    name: 'OrderDetail',
    component: () => import('../views/user/OrderDetail.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('../views/user/Profile.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/settings',
    name: 'Settings',
    component: () => import('../views/user/Settings.vue'),
    meta: { requiresAuth: true }
  },
  {
    path: '/about',
    name: 'About',
    component: () => import('../views/user/About.vue')
  },
  {
    path: '/contact',
    name: 'Contact',
    component: () => import('../views/user/Contact.vue')
  },
  {
    path: '/faq',
    name: 'FAQ',
    component: () => import('../views/user/FAQ.vue')
  },
  {
    path: '/admin',
    component: () => import('../views/admin/AdminLayout.vue'),
    meta: { requiresAuth: true, requiresAdminOrEmployee: true },
    children: [
      {
        path: '',
        name: 'AdminDashboard',
        component: () => import('../views/admin/Dashboard.vue'),
      },
      {
        path: 'products',
        name: 'AdminProducts',
        component: () => import('../views/admin/ProductManagement.vue'),
      },
      {
        path: 'categories',
        name: 'AdminCategories',
        component: () => import('../views/admin/CategoryManagement.vue'),
      },
      {
        path: 'colors',
        name: 'AdminColors',
        component: () => import('../views/admin/ColorManagement.vue'),
      },
      {
        path: 'sizes',
        name: 'AdminSizes',
        component: () => import('../views/admin/SizeManagement.vue'),
      },
      {
        path: 'variants',
        name: 'AdminVariants',
        component: () => import('../views/admin/VariantManagement.vue'),
      },
      {
        path: 'images',
        name: 'AdminImages',
        component: () => import('../views/admin/ImageManagement.vue'),
      },
      {
        path: 'reports',
        name: 'AdminReports',
        component: () => import('../views/admin/Reports.vue'),
      },
      {
        path: 'orders',
        name: 'AdminOrders',
        component: () => import('../views/admin/OrderManagement.vue'),
      },
      {
        path: 'users',
        name: 'AdminUsers',
        component: () => import('../views/admin/UserManagement.vue'),
        meta: { requiresAdmin: true }
      },
      {
        path: 'vouchers',
        name: 'AdminVouchers',
        component: () => import('../views/admin/VoucherManagement.vue'),
        meta: { requiresAdmin: true }
      },
      {
        path: 'discounts',
        name: 'AdminDiscounts',
        component: () => import('../views/admin/DiscountManagement.vue'),
        meta: { requiresAdmin: true }
      },
      {
        path: 'brand-banners',
        name: 'AdminBrandBanners',
        component: () => import('../views/admin/BrandBannerManagement.vue'),
        meta: { requiresAdmin: true }
      },
      {
        path: 'hero-images',
        name: 'AdminHeroImages',
        component: () => import('../views/admin/HeroImageManagement.vue'),
        meta: { requiresAdmin: true }
      },
    ]
  },
  {
    path: '/403',
    name: 'Forbidden',
    component: () => import('../views/Forbidden.vue')
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('../views/NotFound.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      return { top: 0 }
    }
  }
})

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()

  // Check if token is expired
  if (authStore.isAuthenticated && authStore.isTokenExpired()) {
    authStore.logout()
    if (to.meta.requiresAuth) {
      return next('/login')
    }
  }

  // Guest routes (login, register)
  if (to.meta.requiresGuest && authStore.isAuthenticated) {
    return next('/')
  }

  // Protected routes
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return next('/login')
  }

  // Admin only routes
  if (to.meta.requiresAdmin && !authStore.isAdmin) {
    return next('/403')
  }

  // Admin or Employee routes
  if (to.meta.requiresAdminOrEmployee && !authStore.isAdminOrEmployee) {
    return next('/403')
  }

  // Customer routes
  if (to.meta.requiresCustomer && !authStore.isCustomer) {
    return next('/403')
  }

  next()
})

export default router

