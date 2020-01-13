import { date } from "quasar";

export default function() {
	if (!this.material || !this.comments) return false;

	if (!this.$store.state.auth.user) return false;

	if (this.category.categoryPersonalAccess.MaterialDeleteAny) return true;

	if (this.material.authorId !== this.$store.state.auth.user.id) return false;

	if (
		!this.category.categoryPersonalAccess.MaterialDeleteOwnIfHasReplies &&
		this.comments.length >= 1 &&
		!this.checkLastOwn(this.comments[0])
	)
		return false;

	if (!this.category.categoryPersonalAccess.MaterialDeleteOwnIfTimeNotExceeded) {
		const now = new Date();
		const publish = this.material.publishDate;
		const til = date.addToDate(publish, {
			minutes: config.Materials.TimeToOwnDeleteInMinutes
		});
		if (til < now) return false;
	}

	return !!this.category.categoryPersonalAccess.MaterialDeleteOwn;
}
