<template>
  <div>
    <router-link to="/consultas" style="font-size:14px; color:#1a73e8">← Volver a consultas</router-link>
    <div v-if="consulta" class="card" style="margin-top: 20px; max-width: 560px">
      <h2>Consulta #{{ consulta.id }}</h2>
      <div class="detail-row"><span class="label">Afiliado</span><span>{{ consulta.afiliado?.nombreCompleto }}</span></div>
      <div class="detail-row"><span class="label">DNI</span><span>{{ consulta.afiliado?.dni }}</span></div>
      <div class="detail-row"><span class="label">Profesional</span><span>{{ consulta.profesional?.nombreCompleto }}</span></div>
      <div class="detail-row"><span class="label">Especialidad</span><span>{{ consulta.profesional?.especialidad }}</span></div>
      <div class="detail-row"><span class="label">Fecha y hora</span><span>{{ formatFecha(consulta.fechaHora) }}</span></div>
      <div class="detail-row"><span class="label">Modalidad</span><span>{{ consulta.modalidad }}</span></div>
      <div class="detail-row"><span class="label">Estado</span><span>{{ consulta.estado }}</span></div>
      <div class="detail-row"><span class="label">Motivo</span><span>{{ consulta.motivo }}</span></div>

      <div style="margin-top: 24px">
        <div class="form-group">
          <label>Cambiar estado</label>
          <select v-model="nuevoEstado">
            <option v-for="e in estados" :key="e" :value="e">{{ e }}</option>
          </select>
        </div>
        <button @click="cambiarEstado" style="margin-bottom: 16px">Actualizar estado</button>
      </div>

      <div style="display: flex; gap: 10px">
        <button @click="registrarAtencion">Registrar atención</button>
        <button class="btn-danger" @click="cancelar">Cancelar consulta</button>
        <button @click="marcarInasistencia">Marcar inasistencia</button>
      </div>
    </div>
    <p v-else>Cargando...</p>
  </div>
</template>

<script>
import { consultasApi } from '../services/api'

export default {
  name: 'ConsultaDetalle',
  data() {
    return {
      consulta: null,
      nuevoEstado: 'Pendiente',
      estados: ['Pendiente', 'Confirmado', 'Cancelado', 'Atendido', 'Inasistencia']
    }
  },
  async mounted() {
    try {
      const res = await consultasApi.getById(this.$route.params.id)
      this.consulta = res.data
      this.nuevoEstado = this.consulta.estado
    } catch {
      alert('Error al procesar la solicitud')
    }
  },
  methods: {
    formatFecha(fecha) {
      return new Date(fecha).toLocaleString('es-AR')
    },
    async cambiarEstado() {
      try {
        const res = await consultasApi.actualizarEstado(this.consulta.id, { estado: this.nuevoEstado })
        this.consulta = res.data
      } catch {
        alert('Error al procesar la solicitud')
      }
    },
    async registrarAtencion() {
      try {
        const res = await consultasApi.registrarAtencion(this.consulta.id)
        this.consulta = res.data
        this.nuevoEstado = this.consulta.estado
      } catch {
        alert('Error al procesar la solicitud')
      }
    },
    async cancelar() {
      await consultasApi.cancelar(this.consulta.id)
    },
    async marcarInasistencia() {
      await consultasApi.marcarInasistencia(this.consulta.id)
    }
  }
}
</script>

<style scoped>
.detail-row {
  display: flex;
  padding: 10px 0;
  border-bottom: 1px solid #f0f0f0;
  font-size: 14px;
}
.label {
  width: 130px;
  font-weight: 600;
  color: #666;
  flex-shrink: 0;
}
</style>
