﻿using System.ComponentModel.DataAnnotations;

namespace SistemaGestionEntities;

public class Venta
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo Comentarios es requerido.")]
    [MaxLength(200, ErrorMessage = "El Comentario no puede tener más de 200 caracteres.")]
    public string? Comentarios { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El campo Id de Usuario debe ser mayor a 0.")]
    public Usuario? Usuario { get; set; }

    public List<Producto>? Productos{ get; set; }
    
    public int UsuarioId { get; set; }

}