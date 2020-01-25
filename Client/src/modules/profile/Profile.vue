<template>
	<SunPage class="profile page-padding page-padding-top">
		<template v-if="user">
			<div class="profile__container">
				<div class="profile__img-block flex column">
					<img
						class="profile__photo"
						width="300"
						height="300"
						:src="$avatarPath(user.photo)"
					/>
					<div
						v-if="messageButtons"
						class="profile__private-messages-block flex q-mt-sm"
					>
						<q-btn
							no-caps
							class="shadow-1 grow"
							color="info"
							:disable="!canPrivateMessage"
							:to="{
								path: '/SendPrivateMessage'.toLowerCase(),
								query: { userId: user.id, userName: user.name, userLink: user.link }
							}"
							dense
							:icon="$iconsSet.Profile.envelope"
							:label="$tl('sendPrivateMessageBtn')"
						/>
						<q-btn
							:color="!user.iBannedHim ? 'info' : 'negative'"
							class="profile__bun-btn shadow-1 q-ml-sm"
							dense
							v-if="!user.noBannable"
							:icon="$iconsSet.Profile.menu"
						>
							<q-menu>
								<q-btn
									no-caps
									v-close-popup
									color="negative"
									v-if="!user.iBannedHim"
									@click="ban"
									:icon="$iconsSet.Profile.ban"
									:label="$tl('banBtn')"
								/>
								<q-btn
									no-caps
									v-close-popup
									color="positive"
									v-else
									@click="unBan"
									:icon="$iconsSet.Profile.unBan"
									:label="$tl('unBanBtn')"
								/>
							</q-menu>
						</q-btn>
					</div>
				</div>
				<div>
					<PageHeader :title="user.name" />

					<div v-if="self" class="profile__roles-info q-mb-lg">
						{{ $tl("roles") }}:
						<template v-for="role of roles">
							<span class="profile__roles-info-role">{{ role }}</span
							><span class="profile__roles-info-comma">, </span>
						</template>
					</div>

					<div
						class="profile__information profile__text q-mb-lg"
						v-html="user.information"
					></div>

					<div class="profile__footer-info">
						<div class="profile__registered grow">
							{{ $tl("registered") }}: {{ $formatDateOnly(user.registeredDate) }}
						</div>
						<div class="profile__visits">
							<q-icon :name="$iconsSet.Profile.visits" class="q-mr-sm" />
							{{ user.profileVisitsCount }}
						</div>
					</div>

					<div class="text-center q-mt-lg">
						<q-btn
							no-caps
							:label="$tl('roles')"
							color="info"
							:icon="$iconsSet.Profile.roles"
							@click="$refs.dialog.show()"
						/>
					</div>
				</div>
			</div>
		</template>

		<LoaderWait v-else />

		<q-dialog ref="dialog">
			<div class="bg-white">
				<ProfileRoles class="profile__roles q-pa-md" :userId="user.id" />
				<q-btn
					color="warning"
					class="full-width"
					:label="$tl('closeRoles')"
					@click="$refs.dialog.hide()"
				/>
			</div>
		</q-dialog>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { prepareLocalLinks } from "sun";

export default {
	name: "Profile",
	mixins: [Page],
	props: {
		link: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			user: null
		};
	},
	computed: {
		self() {
			if (!this.user) return false;
			return (
				this.$route.name === "ProfileInSettings" &&
				this.user.id === this.$store?.state?.auth?.user?.id
			);
		},
		breadcrumbsCategory() {
			return this.$getBreadcrumbs(this.self ? "Personal" : "Users");
		},
		roles() {
			return this.$store?.state?.auth?.roles;
		},
		canPrivateMessage() {
			const from = this.$store?.state?.auth?.user;
			if (!from) return false;
			if (this.user.heBannedMe || this.user.iBannedHim) return false;
			return from.id !== this.user?.id;
		},
		messageButtons() {
			const from = this.$store?.state?.auth?.user;
			if (!from) return false;
			return from.id !== this.user?.id;
		},
		canEditRoles() {
			return this.$store?.state?.auth?.roles?.some(x => x === "Admin");
		}
	},
	watch: {
		link: "loadData"
	},
	methods: {
		prepareLocalLinks() {
			prepareLocalLinks.call(this, this.$el, "profile__text");
		},
		ban() {
			this.$request(this.$Api.Profile.BanUser, {
				userId: this.user.id
			}).then(async response => {
				await this.loadData();
				const msg = this.$tl("banNotify", [this.user.name]);
				this.$successNotify(msg);
			});
		},
		unBan() {
			this.$request(this.$Api.Profile.UnBanUser, {
				userId: this.user.id
			}).then(async response => {
				await this.loadData();
				const msg = this.$tl("unBanNotify", [this.user.name]);
				this.$successNotify(msg);
			});
		},
		loadData() {
			return this.$request(this.$Api.Profile.GetProfile, {
				link: this.link
			}).then(response => {
				this.user = response.data;
				this.title = this.user.name;
				this.$nextTick(() => {
					this.prepareLocalLinks();
				});
			});
		}
	},
	beforeCreate() {
		this.$options.components.ProfileRoles = require("sun").ProfileRoles;
	},
	created() {
		this.loadData();
	}
};
</script>

<style lang="scss">
.profile__container {
	display: flex;
}

.profile__img-block {
	margin-right: 20px;
}

.profile__user-name {
	margin: 0 0 14px 0;
	font-size: 2.2rem;
}

.profile__private-messages-block {
	padding-right: 2px;
	padding-left: 2px;
	align-items: center;
	width: 300px;
}

.profile__bun-btn {
	padding-left: 10px !important;
	padding-right: 10px !important;
}

.profile__roles-info {
	:nth-last-child(1) {
		display: none;
	}
}

.profile__roles-info-role {
	font-weight: bold;
}

.profile__footer-info {
	display: flex;
	font-style: italic;
	color: $grey-8;

	div {
		display: flex;
		align-items: center;
	}
}

.profile__expansion-item-roles {
	border-radius: 12px;
	margin-top: 30px;
	border: 1px solid silver;
	max-width: 500px;
}

@media (max-width: 900px) {
	.profile__container {
		flex-direction: column;
	}

	.profile__img-block {
		align-content: center;
	}
}
</style>
