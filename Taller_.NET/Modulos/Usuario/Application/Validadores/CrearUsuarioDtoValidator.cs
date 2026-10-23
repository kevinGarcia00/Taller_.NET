using FluentValidation;
using Taller_.NET.Modulos.Usuario;
using Taller_.NET.Modulos.Usuario.Application.Dtos;

namespace Taller_.NET.Modulos.Usuario.Application.Validadores
{
    public class CrearUsuarioDtoValidator : AbstractValidator<CrearUsuarioDto>
    {
        public const int LongitudMinimaContrasena = 8;
        public const int LongitudMaximaContrasena = 128;

        public CrearUsuarioDtoValidator()
        {
            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("El nombre del usuario es obligatorio.")
                .MaximumLength(Usuario.LongitudMaximaNombre)
                    .WithMessage($"El nombre del usuario no puede superar los {Usuario.LongitudMaximaNombre} caracteres.");

            RuleFor(dto => dto.Apellido)
                .NotEmpty().WithMessage("El apellido del usuario es obligatorio.")
                .MaximumLength(Usuario.LongitudMaximaApellido)
                    .WithMessage($"El apellido del usuario no puede superar los {Usuario.LongitudMaximaApellido} caracteres.");

            RuleFor(dto => dto.Correo)
                .NotEmpty().WithMessage("El correo del usuario es obligatorio.")
                .MaximumLength(Usuario.LongitudMaximaCorreo)
                    .WithMessage($"El correo del usuario no puede superar los {Usuario.LongitudMaximaCorreo} caracteres.")
                .EmailAddress().WithMessage("El correo del usuario debe cumplir con el formato usuario@dominio.com.");

            RuleFor(dto => dto.Telefono)
                .MaximumLength(Usuario.LongitudMaximaTelefono)
                    .WithMessage($"El telefono del usuario no puede superar los {Usuario.LongitudMaximaTelefono} caracteres.");

            RuleFor(dto => dto.RolesIds)
                .NotEmpty().WithMessage("Debe asignarse al menos un rol al usuario.")
                .Must(rolesIds => rolesIds.Distinct().Count() == rolesIds.Count)
                    .WithMessage("No se pueden asignar roles repetidos al usuario.");
        }
    }
}
