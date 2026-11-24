<template>
  <div class="card" style="max-width: 560px">
    <h2>Nueva consulta</h2>
    <form @submit.prevent="guardar">
      <div class="form-group">
        <label>Afiliado</label>
        <select v-model="form.afiliadoId">
          <option value="">Seleccioná un afiliado</option>
          <option v-for="a in afiliados" :key="a.id" :value="a.id">
            {{ a.nombreCompleto }} — DNI {{ a.dni }}
          </option>
        </select>
      </div>
      <div class="form-group">
        <label>Profesional</label>
        <select v-model="form.profesionalId">
          <option value="">Seleccioná un profesional</option>
          <option v-for="p in profesionales" :key="p.id" :value="p.id">
            {{ p.nombreCompleto }} — {{ p.especialidad }}
          </option>
        </select>
      </div>
      <div class="form-group">
        <label>Modalidad</label>
        <select v-model="form.modalidad">
          <option value="Presencial">Presencial</option>
          <option value="Virtual">Virtual</option>
        </select>
      </div>
      <div class="form-group">
        <label>Fecha y hora</label>
        <input type="datetime-local" v-model="form.fechaHora" />
      </div>
      <div class="form-group">
        <label>Motivo</label>
        <input type="text" v-model="form.motivo" placeholder="Motivo de la consulta" />
      </div>
      <button type="submit">Confirmar consulta</button>
    </form>
  </div>
</template>

<script>
import { consultasApi, afiliadosApi, profesionalesApi } from '../services/api'

export default {
  name: 'ConsultaNueva',
  data() {
    return {
      form: {
        afiliadoId: '',
        profesionalId: '',
        modalidad: 'Presencial',
        fechaHora: '',
        motivo: ''
      },
      afiliados: [],
      profesionales: []
    }
  },
  async mounted() {
    try {
      const [aRes, pRes] = await Promise.all([afiliadosApi.getAll(), profesionalesApi.getAll()])
      this.afiliados = aRes.data
      this.profesionales = pRes.data
    } catch {
      alert('Error al procesar la solicitud')
    }
  },
  methods: {
    async guardar() {
      try {
        await consultasApi.create({
          afiliadoId: Number(this.form.afiliadoId),
          profesionalId: Number(this.form.profesionalId),
          modalidad: this.form.modalidad,
          fechaHora: this.form.fechaHora,
          motivo: this.form.motivo
        })
        this.$router.push('/consultas')
      } catch {
        alert('Error al procesar la solicitud')
      }
    }
  }
}
</script>
