using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace Taller_.NET.Components.Shared.Forms.Usuarios;

/// <summary>
/// Maneja la validacion de los formularios del modulo Usuario sobre el EditContext.
/// </summary>
public class ValidacionFormulario
{
    private EditContext? _editContext;
    private ValidationMessageStore? _almacen;

    /// <summary>
    /// Asocia el formulario al EditContext que RadzenTemplateForm crea para el.
    /// </summary>
    public void Configurar(EditContext editContext)
    {
        if (ReferenceEquals(_editContext, editContext))
        {
            return;
        }

        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= OnCampoModificado;
        }

        _editContext = editContext;

        // El almacen debe ser siempre la misma instancia: en .NET cada ValidationMessageStore
        // se asocia a los campos del EditContext, y crear uno nuevo por llamada dejaria los
        // mensajes anteriores colgados (el formulario quedaria bloqueado).
        _almacen = new ValidationMessageStore(editContext);

        _editContext.OnFieldChanged += OnCampoModificado;
    }

    /// <summary>
    /// Valida el modelo, deja los mensajes en el EditContext y devuelve los errores encontrados.
    /// </summary>
    public async Task<IReadOnlyList<ValidationFailure>> ValidarAsync<T>(
        IValidator<T> validador,
        T modelo,
        IReadOnlyDictionary<string, FieldIdentifier> campos)
    {
        var almacen = ObtenerAlmacen();
        almacen.Clear();

        var resultado = await validador.ValidateAsync(modelo);

        foreach (var error in resultado.Errors)
        {
            if (campos.TryGetValue(error.PropertyName, out var campo))
            {
                almacen.Add(campo, error.ErrorMessage);
            }
        }

        _editContext!.NotifyValidationStateChanged();

        return resultado.Errors;
    }

    /// <summary>
    /// Notifica que un campo cambio para que se limpien sus mensajes de validacion.
    /// </summary>
    public void NotificarCampoModificado(FieldIdentifier campo)
    {
        _editContext!.NotifyFieldChanged(campo);
    }

    private void OnCampoModificado(object? sender, FieldChangedEventArgs args)
    {
        // Al corregir un campo se quita su mensaje para que el formulario vuelva
        // a permitir el envio.
        ObtenerAlmacen().Clear(args.FieldIdentifier);

        _editContext!.NotifyValidationStateChanged();
    }

    private ValidationMessageStore ObtenerAlmacen()
    {
        _almacen ??= new ValidationMessageStore(_editContext
            ?? throw new InvalidOperationException("El formulario todavia no fue configurado."));

        return _almacen;
    }
}
