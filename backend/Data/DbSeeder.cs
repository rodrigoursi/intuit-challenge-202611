using ConsultasPlan.Models;

namespace ConsultasPlan.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Sedes.Any()) return;

        var hoy = DateTime.Now;
        var y = hoy.Year;

        var sedes = new[]
        {
            new Sede { Nombre = "Palermo",   Direccion = "Av. Santa Fe 3500, CABA" },
            new Sede { Nombre = "Belgrano",  Direccion = "Av. Cabildo 1200, CABA"  },
            new Sede { Nombre = "San Telmo", Direccion = "Defensa 850, CABA"       }
        };
        db.Sedes.AddRange(sedes);
        db.SaveChanges();

        var profesionales = new[]
        {
            new Profesional { NombreCompleto = "Dr. Alejandro Ríos",   Especialidad = "Clínica Médica", SedeId = sedes[0].Id },
            new Profesional { NombreCompleto = "Dra. Valentina Mora",  Especialidad = "Pediatría",      SedeId = sedes[0].Id },
            new Profesional { NombreCompleto = "Dr. Carlos Benítez",   Especialidad = "Cardiología",    SedeId = sedes[1].Id },
            new Profesional { NombreCompleto = "Lic. Julieta Arana",   Especialidad = "Nutrición",      SedeId = sedes[1].Id },
            new Profesional { NombreCompleto = "Lic. Tomás Ibarra",    Especialidad = "Nutrición",      SedeId = sedes[2].Id },
            new Profesional { NombreCompleto = "Dr. Martín Gutiérrez", Especialidad = "Traumatología",  SedeId = sedes[2].Id }
        };
        db.Profesionales.AddRange(profesionales);
        db.SaveChanges();

        var clinica = profesionales[0];
        var cardio  = profesionales[2];
        var nutri1  = profesionales[3];
        var nutri2  = profesionales[4];

        Afiliado A(string nombre, string dni, string email, string tel, int usadas, bool bloqueado, DateTime? fechaBloqueo, DateTime alta) =>
            new Afiliado
            {
                NombreCompleto = nombre, DNI = dni, Email = email, Telefono = tel, Plan = "Plan Integral",
                ConsultasUsadas = usadas, Bloqueado = bloqueado, FechaBloqueo = fechaBloqueo,
                createdAt = alta, isActive = true
            };

        var afiliados = new[]
        {
            A("Gonzalo Herrera",   "28445123", "gonzalo.herrera@gmail.com", "1145678901", 0, false, null, new DateTime(y - 1, 11, 4)),
            A("Mariana López",     "31202456", "mariana.lopez@hotmail.com", "1167891234", 5, false, null, new DateTime(y - 1, 12, 14)),
            A("Roberto Quintana",  "25789012", "roberto.q@gmail.com",       "1156789012", 6, false, null, new DateTime(y - 1, 12, 23)),
            A("Sofía Vargas",      "35123789", "sofia.vargas@yahoo.com",    "1178901234", 1, false, null, new DateTime(y - 1, 12, 30)),
            A("Diego Fernández",   "29876543", "diegofernandez@gmail.com",  "1134567890", 1, false, null, new DateTime(y - 1, 10, 25)),
            A("Laura Giménez",     "33456789", "lauragimenez@gmail.com",    "1189012345", 0, false, null, new DateTime(y, 1, 4)),
            A("Facundo Sosa",      "27345678", "facundo.sosa@outlook.com",  "1112345678", 0, true,  new DateTime(y, 4, 19), new DateTime(y - 1, 10, 27)),
            A("Carolina Méndez",   "36901234", "carolina.mendez@gmail.com", "1190123456", 1, false, null, new DateTime(y - 2, 3, 14))
        };
        db.Afiliados.AddRange(afiliados);
        db.SaveChanges();

        var consultas = new List<Consulta>();

        void C(Afiliado a, Profesional p, DateTime fecha, EstadoConsulta estado, ModalidadConsulta modalidad, string motivo)
        {
            consultas.Add(new Consulta
            {
                AfiliadoId = a.Id,
                ProfesionalId = p.Id,
                FechaHora = fecha,
                Estado = estado,
                Modalidad = modalidad,
                FechaCreacion = fecha < hoy ? fecha.AddDays(-5) : hoy,
                Motivo = motivo
            });
        }

        // 1: sin historial de nutrición
        C(afiliados[0], clinica, hoy.Date.AddDays(10).AddHours(9), EstadoConsulta.Pendiente, ModalidadConsulta.Presencial, "Control anual");

        // 2: 5 consultas de nutrición en el año (junio a octubre)
        for (var m = 6; m <= 10; m++)
            C(afiliados[1], nutri1, new DateTime(y, m, 10, 11, 0, 0), EstadoConsulta.Atendido, ModalidadConsulta.Presencial, "Seguimiento nutricional");

        // 3: 6 consultas de nutrición en el año (enero a junio)
        for (var m = 1; m <= 6; m++)
            C(afiliados[2], nutri2, new DateTime(y, m, 10, 10, 0, 0), EstadoConsulta.Atendido, ModalidadConsulta.Presencial, "Seguimiento nutricional");

        // 4: ya tuvo una consulta de nutrición en el mes corriente
        C(afiliados[3], nutri1, new DateTime(y, hoy.Month, 1, 10, 0, 0), EstadoConsulta.Atendido, ModalidadConsulta.Presencial, "Plan alimentario");

        // 5: una virtual atendida y una presencial cancelada
        var mesAnterior = hoy.AddMonths(-1);
        var dosMesesAtras = hoy.AddMonths(-2);
        C(afiliados[4], nutri1, new DateTime(mesAnterior.Year, mesAnterior.Month, 15, 16, 0, 0), EstadoConsulta.Atendido, ModalidadConsulta.Virtual, "Consulta de seguimiento");
        C(afiliados[4], nutri2, new DateTime(dosMesesAtras.Year, dosMesesAtras.Month, 15, 16, 0, 0), EstadoConsulta.Cancelado, ModalidadConsulta.Presencial, "Primera consulta");

        // 6: una inasistencia y una consulta futura
        C(afiliados[5], nutri2, new DateTime(mesAnterior.Year, mesAnterior.Month, 20, 9, 0, 0), EstadoConsulta.Inasistencia, ModalidadConsulta.Presencial, "Control de peso");
        C(afiliados[5], nutri1, hoy.Date.AddDays(20).AddHours(15), EstadoConsulta.Pendiente, ModalidadConsulta.Presencial, "Control de peso");

        // 7: afiliado bloqueado
        C(afiliados[6], clinica, new DateTime(y, 2, 12, 9, 0, 0), EstadoConsulta.Cancelado, ModalidadConsulta.Presencial, "Chequeo general");

        // 8: consulta de nutrición del año anterior y una de cardiología de este año
        C(afiliados[7], nutri1, new DateTime(y - 1, 12, 15, 11, 0, 0), EstadoConsulta.Atendido, ModalidadConsulta.Presencial, "Plan alimentario");
        C(afiliados[7], cardio, new DateTime(y, 3, 8, 11, 0, 0), EstadoConsulta.Atendido, ModalidadConsulta.Presencial, "Electrocardiograma de rutina");

        db.Consultas.AddRange(consultas);
        db.SaveChanges();
    }
}
