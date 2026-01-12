<template>
  <div class="relative w-full h-64 md:h-80 lg:h-96 overflow-hidden rounded-xl">
    <div
      v-for="(banner, index) in banners"
      :key="banner.id"
      :class="[
        'absolute inset-0 transition-all duration-1000 ease-in-out',
        index === currentIndex ? 'opacity-100 translate-y-0' : 
        index < currentIndex ? 'opacity-0 -translate-y-full' : 
        'opacity-0 translate-y-full'
      ]"
    >
      <img
        :src="banner.imageUrl || '/placeholder-banner.jpg'"
        :alt="`${brand} banner ${index + 1}`"
        class="w-full h-full object-cover"
        @error="handleImageError"
      />
      <div class="absolute inset-0 bg-gradient-to-t from-black/30 to-transparent"></div>
    </div>
    
    <!-- Navigation Dots -->
    <div v-if="banners.length > 1" class="absolute bottom-4 left-1/2 transform -translate-x-1/2 flex space-x-2">
      <button
        v-for="(banner, index) in banners"
        :key="banner.id"
        @click="goToSlide(index)"
        :class="[
          'w-2 h-2 rounded-full transition-all duration-300',
          index === currentIndex ? 'bg-white w-8' : 'bg-white/50'
        ]"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

const props = defineProps({
  banners: {
    type: Array,
    required: true,
    default: () => []
  },
  brand: {
    type: String,
    required: true
  },
  autoPlay: {
    type: Boolean,
    default: true
  },
  interval: {
    type: Number,
    default: 5000 // 5 seconds
  }
})

const currentIndex = ref(0)
let intervalId = null

const goToSlide = (index) => {
  currentIndex.value = index
  resetAutoPlay()
}

const nextSlide = () => {
  if (props.banners.length === 0) return
  currentIndex.value = (currentIndex.value + 1) % props.banners.length
}

const resetAutoPlay = () => {
  if (intervalId) {
    clearInterval(intervalId)
  }
  if (props.autoPlay && props.banners.length > 1) {
    intervalId = setInterval(nextSlide, props.interval)
  }
}

const handleImageError = (e) => {
  e.target.src = '/placeholder-banner.jpg'
}

onMounted(() => {
  if (props.autoPlay && props.banners.length > 1) {
    intervalId = setInterval(nextSlide, props.interval)
  }
})

onUnmounted(() => {
  if (intervalId) {
    clearInterval(intervalId)
  }
})
</script>

