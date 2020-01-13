export default function() {
	const deleteDialogTitle = this.$tl("deleteDialogTitle");
	const deleteDialogMessage = this.$tl("deleteDialogMessage");
	const okBtn = this.$tl("deleteDialogOk");
	const cancelBtn = this.$tl("deleteDialogCancel");
	this.$q
		.dialog({
			title: deleteDialogTitle,
			message: deleteDialogMessage,
			ok: okBtn,
			cancel: cancelBtn
		})
		.onOk(() => {
			this.$request(this.$Api.Materials.Delete, {
				id: this.material.id
			})
				.then(() => {
					const deleteSuccessMsg = this.$tl("deleteSuccess");
					this.$successNotify(deleteSuccessMsg);
					this.$router.push(this.category.getRoute());
				})
				.catch(x => {
					console.log("error", x);
				});
		});
}
