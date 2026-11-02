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

            RuleFor(dto => dto.Contrasena)
                .NotEmpty().WithMessage("La contrasena del usuario es obligatoria.")
                .MinimumLength(LongitudMinimaContrasena)
                    .WithMessage($"La contrasena debe tener al menos {LongitudMinimaContrasena} caracteres.")
                .MaximumLength(LongitudMaximaContrasena)
                    .WithMessage($"La contrasena no puede superar los {LongitudMaximaContrasena} caracteres.")
                .Matches("[A-Za-z]").WithMessage("La contrasena debe incluir al menos una letra.")
                .Matches("[0-9]").WithMessage("La contrasena debe incluir al menos un numero.");

            RuleFor(dto => dto.RolesIds)
                .NotEmpty().WithMessage("Debe asignarse al menos un rol de Auth0 al usuario.")
                .Must(rolesIds => rolesIds.All(rolId => !string.IsNullOrWhiteSpace(rolId)))
                    .WithMessage("Los identificadores de roles no pueden estar vacios.")
                .Must(rolesIds => rolesIds.Distinct(StringComparer.Ordinal).Count() == rolesIds.Count)
                    .WithMessage("No se pueden asignar roles repetidos al usuario.");
        }
    }
}
