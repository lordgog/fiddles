public static Task RenderPartialForAsync<TModel, TProperty>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string partialName)
{
    ModelExpressionProvider modelExpressionProvider = new ModelExpressionProvider(html.MetadataProvider);

    string name = modelExpressionProvider.GetExpressionText(expression);
    string modelName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

    ModelExpression modelExpression = modelExpressionProvider.CreateModelExpression(html.ViewData, expression);

    object model = modelExpression.Model;

    if (partialName == null)
    {
        partialName = modelExpression.Metadata.TemplateHint == null
            ? typeof(TProperty).Name
            : modelExpression.Metadata.TemplateHint;
    }

    ViewDataDictionary viewData = new ViewDataDictionary(html.ViewData);
    viewData.TemplateInfo.HtmlFieldPrefix = modelName;

    return html.RenderPartialAsync(partialName, model, viewData);
}