<template>
  <div>
    <h2>Consultas</h2>
    <table v-if="consultas.length">
      <thead>
        <tr>
          <th>#</th>
          <th>Afiliado</th>
          <th>Profesional</th>
          <th>Especialidad</th>
          <th>Fecha y hora</th>
          <th>Modalidad</th>
          <th>Estado</th>
          <th>Motivo</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="consulta in consultas" :key="consulta.id">
          <td>{{ consulta.id }}</td>
          <td>{{ consulta.afiliado?.nombreCompleto }}</td>
          <td>{{ consulta.profesional?.nombreCompleto }}</td>
          <td>{{ consulta.profesional?.especialidad }}</td>
          <td>{{ formatFecha(consulta.fechaHora) }}</td>
          <td>{{ consulta.modalidad }}</td>
          <td>
            <span :class="['badge', `badge-${consulta.estado?.toLowerCase()}`]">{{ consulta.estado }}</span>
          </td>
          <td>{{ consulta.motivo }}</td>
          <td>
            <router-link :to="`/consultas/${consulta.id}`">Ver</router-link>
            <button class="btn-danger" style="margin-left: 8px" @click="cancelar(consulta.id)">Cancelar</button>
          </td>
        </tr>
      </tbody>
    </table>
    <p v-else>No hay consultas registradas.</p>
  </div>
</template>

<script>
import { consultasApi } from '../services/api'

export default {
  name: 'ConsultasList',
  data() {
    return {
      consultas: []
    }
  },
  async mounted() {
    try {
      const res = await consultasApi.getAll()
      this.consultas = res.data
    } catch {
      alert('Error al procesar la solicitud')
    }
  },
  methods: {
    formatFecha(fecha) {
      return new Date(fecha).toLocaleString('es-AR')
    },
    async cancelar(id) {
      await consultasApi.cancelar(id)
    }
  }
}
</script>

<style scoped>
.badge {
  padding: 3px 10px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 600;
}
.badge-pendiente    { background: #fff3cd; color: #856404; }
.badge-confirmado   { background: #d4edda; color: #155724; }
.badge-cancelado    { background: #f8d7da; color: #721c24; }
.badge-atendido     { background: #d1ecf1; color: #0c5460; }
.badge-inasistencia { background: #e2e3e5; color: #383d41; }
</style>
