// Copyright (c) 2016-2017 Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Markdig.Syntax;

namespace Markdig.Renderers.Json.Blocks
{
    /// <summary>
    /// An XAML renderer for a <see cref="HeadingBlock"/>.
    /// </summary>
    /// <seealso cref="JsonObjectRenderer{TObject}" />
    public class HeadingRenderer : JsonObjectRenderer<HeadingBlock>
    {
        protected override void Write([NotNull] JsonRenderer renderer, [NotNull] HeadingBlock obj)
        {
            renderer.EnsureLine();
            renderer.Write("{ ");
            renderer.Write($"\"type\": \"heading\", \"level\": \"{obj.Level}\", \"value\": \"");
            renderer.WriteLeafInline(obj);
            renderer.Write("\" }");
        }
    }
}