<template>
  <div class="flex min-h-screen items-center justify-center bg-gray-100 px-4 py-12">
    <div class="text-center">
      <h1 class="text-9xl font-bold text-indigo-600">403</h1>
      <h2 class="mt-4 text-3xl font-bold text-gray-900">Truy cập bị từ chối</h2>
      <p class="mt-2 text-lg text-gray-600">
        Bạn không có quyền truy cập vào trang này.
      </p>
      <div class="mt-8 flex justify-center space-x-4">
        <router-link
          to="/"
          class="rounded-lg bg-indigo-600 px-6 py-3 text-white hover:bg-indigo-700"
        >
          Về trang chủ
        </router-link>
        <button
          @click="$router.go(-1)"
          class="rounded-lg border border-gray-300 px-6 py-3 text-gray-700 hover:bg-gray-50"
        >
          Quay lại
        </button>
      </div>
    </div>
  </div>
</template>

