using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StepOnThat.Infrastructure;

namespace StepOnThat
{
    public class DynamicPromptForm
    {
        private readonly IHasProperties properties;
        private const int controlHeight = 32;
        private readonly Font defaultFont = new Font("Tahoma", 11f, FontStyle.Regular);
        private Form form;
        private const int formWidth = 400;
        private const string promptPropertyText = ":prompt";
        private const int spaceBetweenControls = 12;

        public DynamicPromptForm(IHasProperties properties)
        {
            this.properties = properties;
            Application.EnableVisualStyles();
        }

        public async Task PromptForVariablesWithUI(Func<Task> onFormSubmitted)
        {
            form = CreateForm();

            foreach (var property in properties)
            {
                AddLabelAndTextboxForProperty(property, properties.IndexOf(property));
            }

            await AddSubmitButton(onFormSubmitted);

            AddCloseButton();

            form.ShowDialog();
        }

        private void AddCloseButton()
        {
            var closeButton = new Button
            {
                Text = "Exit",
                Size = new Size(formWidth/4, controlHeight),
                Location = new Point((formWidth/4)*2, properties.Count + 1*(controlHeight + spaceBetweenControls)),
                BackColor = Color.White,
                ForeColor = Color.Gray,
                Font = defaultFont
            };
            closeButton.Click += (obj, evt) => form.Close();
            form.Controls.Add(closeButton);
        }

        private async Task AddSubmitButton(Func<Task> onFormSubmitted)
        {
            var submitButton = new Button
            {
                Text = "Submit",
                Size = new Size(formWidth/4, controlHeight),
                Location = new Point((3*formWidth)/4, properties.Count + (controlHeight + spaceBetweenControls)),
                BackColor = Color.White,
                ForeColor = Color.DodgerBlue,
                Font = defaultFont
            };
            form.Controls.Add(submitButton);
            submitButton.Click += async (obj, evt) =>
            {
                form.Close();
                foreach (var control in form.Controls.OfType<TextBox>())
                {
                    if (properties.Any(x => x.Key == control.Name))
                    {
                        properties[control.Name] = control.Text;
                    }
                }
                await onFormSubmitted();
            };
        }

        private void AddLabelAndTextboxForProperty(Property property, int index)
        {
            var verticalPosition = spaceBetweenControls + index *(controlHeight + spaceBetweenControls);
            form.Controls.Add(new Label
            {
                Text = property.Key.Replace(promptPropertyText, ""),
                Size = new Size((formWidth/3), controlHeight),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(spaceBetweenControls, verticalPosition - 5),
                ForeColor = Color.White,
                Padding = new Padding(0, 0, spaceBetweenControls, 0),
                Font = defaultFont
            });
            form.Controls.Add(new TextBox
            {
                Name = property.Key,
                Size = new Size((formWidth/3)*2 - spaceBetweenControls, controlHeight),
                Location = new Point(formWidth/3 + spaceBetweenControls, verticalPosition),
                Font = defaultFont
            });
        }

        private Form CreateForm()
        {
            var f = new Form
            {
                Padding = new Padding(spaceBetweenControls),
                BackColor = Color.DimGray,
                Width = formWidth + spaceBetweenControls,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Height = spaceBetweenControls*2 + (controlHeight*(properties.Count + 1))
            };
            return f;
        }
    }
}