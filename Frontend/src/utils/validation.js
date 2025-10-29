/**
 * Validate email address
 */
export function isValidEmail(email) {
  const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return re.test(email)
}

/**
 * Validate phone number (Vietnam format)
 */
export function isValidPhone(phone) {
  const re = /^(0|\+84)(3|5|7|8|9)[0-9]{8}$/
  return re.test(phone.replace(/\s/g, ''))
}

/**
 * Validate password strength
 */
export function validatePassword(password) {
  const errors = []
  
  if (password.length < 8) {
    errors.push('Mật khẩu phải có ít nhất 8 ký tự')
  }
  
  if (!/[a-z]/.test(password)) {
    errors.push('Mật khẩu phải chứa ít nhất 1 chữ thường')
  }
  
  if (!/[A-Z]/.test(password)) {
    errors.push('Mật khẩu phải chứa ít nhất 1 chữ hoa')
  }
  
  if (!/[0-9]/.test(password)) {
    errors.push('Mật khẩu phải chứa ít nhất 1 số')
  }
  
  return {
    isValid: errors.length === 0,
    errors
  }
}

/**
 * Validate required field
 */
export function required(value, fieldName = 'Trường này') {
  if (!value || (typeof value === 'string' && !value.trim())) {
    return `${fieldName} không được để trống`
  }
  return null
}

/**
 * Validate minimum length
 */
export function minLength(value, min, fieldName = 'Trường này') {
  if (value && value.length < min) {
    return `${fieldName} phải có ít nhất ${min} ký tự`
  }
  return null
}

/**
 * Validate maximum length
 */
export function maxLength(value, max, fieldName = 'Trường này') {
  if (value && value.length > max) {
    return `${fieldName} không được vượt quá ${max} ký tự`
  }
  return null
}

/**
 * Validate numeric value
 */
export function isNumeric(value) {
  return !isNaN(parseFloat(value)) && isFinite(value)
}

/**
 * Validate value is in range
 */
export function inRange(value, min, max) {
  const num = parseFloat(value)
  return num >= min && num <= max
}

/**
 * Validate URL
 */
export function isValidURL(url) {
  try {
    new URL(url)
    return true
  } catch {
    return false
  }
}

