# CompositeCollection

This project aims to implement classes similar to [CompositeCollection](http://msdn.microsoft.com/de-de/library/system.windows.data.compositecollection(v=vs.110).aspx) for Windows Store.

A [NuGet-Package](https://www.nuget.org/packages/Midgard.CompositeCollection/) is also available.

# How to use

<div style="color:Black; background-color:White">

<pre><span style="color:Blue"><</span><span style="color:#A31515">ListBox</span> <span style="color:Red">Name</span><span style="color:Blue">=</span><span style="color:Black">"</span><span style="color:Blue">list</span><span style="color:Black">"</span> <span style="color:Red">Margin</span><span style="color:Blue">=</span><span style="color:Black">"</span><span style="color:Blue">30</span><span style="color:Black">"</span> <span style="color:Blue">></span>
            <span style="color:Blue"><</span><span style="color:#A31515">ListBox.ItemsSource</span><span style="color:Blue">></span>
                <span style="color:Blue"><</span><span style="color:#A31515">mcc</span><span style="color:Blue">:</span><span style="color:#A31515">CompositeCollection</span><span style="color:Blue">></span>
                    <span style="color:Blue"><</span><span style="color:#A31515">mcc</span><span style="color:Blue">:</span><span style="color:#A31515">CompositeCollection.Composition</span><span style="color:Blue">></span>
                        <span style="color:Blue"><</span><span style="color:#A31515">mcc</span><span style="color:Blue">:</span><span style="color:#A31515">CollectionContainer</span> <span style="color:Red">Collection</span><span style="color:Blue">=</span><span style="color:Black">"</span><span style="color:Blue">{Binding Back, Source={StaticResource Object1}}</span><span style="color:Black">"</span> <span style="color:Blue">/></span>
                        <span style="color:Blue"><</span><span style="color:#A31515">ListBoxItem</span><span style="color:Blue">></span>uaeuia<span style="color:Blue"></</span><span style="color:#A31515">ListBoxItem</span><span style="color:Blue">></span>
                    <span style="color:Blue"></</span><span style="color:#A31515">mcc</span><span style="color:Blue">:</span><span style="color:#A31515">CompositeCollection.Composition</span><span style="color:Blue">></span>
                <span style="color:Blue"></</span><span style="color:#A31515">mcc</span><span style="color:Blue">:</span><span style="color:#A31515">CompositeCollection</span><span style="color:Blue">></span>
            <span style="color:Blue"></</span><span style="color:#A31515">ListBox.ItemsSource</span><span style="color:Blue">></span>
        <span style="color:Blue"></</span><span style="color:#A31515">ListBox</span><span style="color:Blue">></span>
</pre>

</div>

For the Itemssource specify the CompositCollection. Add all items you want to display to the Composition property. If you want to add one or more collections, define a CollectionsContainer and set the Collection property.

Be careful to use a StaticResource to bind against. Binding against the Datacontext dose not work.
