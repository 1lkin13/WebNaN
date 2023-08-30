document.querySelector('#ProductPhotoSelect').addEventListener('change', function () {
    let item = document.createElement('div');
    item.className = 'item'
    let img = document.createElement('img');
    img.src = URL.createObjectURL(this.files[0])
    item.appendChild(img)
    item.appendChild(this.cloneNode(true))
    this.value = null
    item.addEventListener('click', function () {
        this.remove()
    })
    
document.querySelector('.photoarea').appendChild(item);

})