# Goods-Sort

Bu yapılan oyun kısa sürede hazırlanmış, Goods Sorting oyununun clone projesidir.

Oyun StartScene sahnesinden başlamaktadır. MainScene’e aktarıldıktan sonra oyunu oynamak için sahnedeki butona basılmalıdır. Butona basıldıktan sonra kullanıcı oyun sahnesine yönlendirilir.

Oyun sahnesinde kullanıcı 3 adet aynı tipteki itemi yanyana getirmeyi başarırsa itemler kaybolur ve belirlenen süre içerisinde bütün itemleri yok etmeyi başarabilirse level atlamış olur.

Oyunda levelleri düzenlemek adına bir LevelEditorScene bulunmaktadır. Bu sahne açıldığında otomatik olarak Level_1 yüklenir. Sol altta görülen scroll view ile istenilen level seçilip üzerinde ayarlamalar yapılabilir. Level ile ilgili bilgiler Resource/Levels klasöründe scriptableobject olarak saklanmaktadır. Bu scriptable objenin içinden level biligleri değiştirildikten sonra LoadLevel butonuna basılırsa yapılan değişiklikler sahnede gösterilir. Sadece kamera ayarları OnValidate fonksiyonuyla çalıştığından anlık değişimler gözlemlenebilir. Create New Level butonuna basıldığında ise otomatik yeni level tanımlanır.
