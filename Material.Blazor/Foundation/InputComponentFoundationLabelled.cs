using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Material.Blazor.Internal
{
    /// <summary>
    /// Inherits from <see cref="InputComponentFoundation{T}"/> adding a Label parameter and functionality to
    /// allow updated labels to be reflected in the DOM via a JSInterop call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class InputComponentFoundationLabelled<T> : InputComponentFoundation<T>
    {
        private string _cachedLabel;
        /// <summary>
        /// Field label.
        /// </summary>
        [Parameter] public string Label { get; set; }


        /// <summary>
        /// Derived components can use this to get a callback from OnParametersSet when the consumer changes
        /// the label. This allows a component to take action with Material Components Web js to update the DOM to reflect
        /// the data change visually.
        /// </summary>
        protected event EventHandler SetLabel;


        /// <inheritdoc/>
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            if (!HasSetInitialParameters)
            {
                // This is the first run
                // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()
                _cachedLabel = Label;
            }

            await base.SetParametersAsync(parameters);
        }
        
        
        /// <inheritdoc/>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            CommonParametersSet();
        }


        /// <inheritdoc/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            CommonParametersSet();
        }


        private void CommonParametersSet()
        {
            if (!EqualityComparer<string>.Default.Equals(_cachedLabel, Label))
            {
                _cachedLabel = Label;
#if Logging
                Logger.LogDebug("OnParametersSet setting _cachedLabel value to '" + Label ?? "null" + "'");
#endif
                if (HasInstantiated)
                {
                    SetLabel?.Invoke(this, null);
                }
            }
        }
    }
}
